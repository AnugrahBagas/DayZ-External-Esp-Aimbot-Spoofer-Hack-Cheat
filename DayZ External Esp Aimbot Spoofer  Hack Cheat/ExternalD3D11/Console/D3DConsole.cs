using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalD3D11.Console
{
    public class D3DConsole
    {
        #region Globals

        // Base
        private DirectXBase d3dBase;
        private TextFormat consoleFont;
        private TextFormat logFont;

        // Caret
        private int caretTick = 0;
        private bool showCaret = true;
        private int caretPos = 0;

        // Command text
        private string commandText = string.Empty;
        private List<string> suggestions = new List<string>();
        private string currentSuggestion = string.Empty;
        private int currentSuggestionIndex = 0;

        // Log text
        private string log = "-- Private Dayz TN Multihack Console -- \n";
        private float logLineHeight = 15f;

        // Available Commands
        public List<ConsoleCommand> Commands = new List<ConsoleCommand>();

        // Log callback delegate (Used to call the log function from anywhere)
        public delegate void LogCallbackEventHandler(string text);

        // Command history
        private List<HistoryCommand> commandHistory = new List<HistoryCommand>();
        private string currentCommandHistory = string.Empty;
        private int currentCommandHistoryIndex = -1;

        #endregion

        #region Properties

        public bool IsVisible = false;

        public float PositionX = 500;
        public float PositionY = 100;
        public float Width = 700;
        public float Height = 350;

        public float BorderThickness = 3;

        public float TextPaddingLeft = 5;

        public float TitleHeight = 20;
        public float TextBoxHeight = 20;

        public float WindowPositionModifier = 10;
        public float WindowSizeModifier = 10;

        public float MinWidth = 300;
        public float MinHeight = 300;

        public Color BackgroundColor = new Color(20, 20, 20, 200);
        public Color BorderColor = new Color(40, 40, 40);
        public Color TitleForeground = Color.LightGray;
        public Color TextBoxBackground = Color.LightGray;
        public Color TextBoxForeground = Color.Black;
        public Color TextBoxSuggestionForeground = Color.DarkGray;
        public Color LogForeground = Color.White;

        #endregion

        #region Constructor

        public D3DConsole(DirectXBase directXBase)
        {
            d3dBase = directXBase;
            consoleFont = new TextFormat(d3dBase.FontBase, "Consolas", 15f);
            logFont = new TextFormat(d3dBase.FontBase, "Consolas", 12f);

            // Default commands
            Commands.Add(new ConsoleCommand("close", new ConsoleCommand.OnCommandEventHandler(HideCommandExecuted), "Closes the console window.", "close"));
            Commands.Add(new ConsoleCommand("clear", new ConsoleCommand.OnCommandEventHandler(ClearCommandExecuted), "Clears the log.", "clear"));
        }

        #endregion

        #region Methods

        public void Draw()
        {
            if (!IsVisible)
                return;

            // Position calculations
            float textBoxPositionY = PositionY + Height - TextBoxHeight - BorderThickness;
            float cursorBasePositionX = PositionX + 15;

            float logTextX = PositionX + BorderThickness + TextPaddingLeft;
            float logTextY = PositionY + TitleHeight + TextPaddingLeft;
            float logTextWidth = Width - (BorderThickness * 2) - (TextPaddingLeft * 2);

            float textBoxX = PositionX + BorderThickness;
            float textBoxY = PositionY + Height - TextBoxHeight - BorderThickness;
            float textBoxWidth = Width - (BorderThickness * 2);

            float consoleSightX = PositionX + BorderThickness + 3;


            // Box
            d3dBase.DrawFilledBox(PositionX, PositionY, Width, Height, BackgroundColor);
            d3dBase.DrawBox(PositionX, PositionY, Width, Height, BorderThickness, BorderColor);

            // Title
            d3dBase.DrawFilledBox(PositionX, PositionY, Width, TitleHeight, BorderColor);
            d3dBase.DrawText("Console", consoleFont, PositionX + TextPaddingLeft, PositionY, TitleForeground);

            // Log
            d3dBase.DrawTextWithWrapping(log, logFont, logTextX, logTextY, logTextWidth, (Height - (TitleHeight + 5) - TextBoxHeight), LogForeground);

            // TextBox
            d3dBase.DrawFilledBox(textBoxX, textBoxY, textBoxWidth, TextBoxHeight, TextBoxBackground);

            // BaseText
            d3dBase.DrawText(">", consoleFont, consoleSightX, textBoxPositionY, TextBoxForeground);

            // Draw Suggestion or history
            string text = string.Empty;
            if (currentCommandHistory != string.Empty)
                text = currentCommandHistory;
            else
                text = currentSuggestion;

            d3dBase.DrawText(text, consoleFont, cursorBasePositionX, textBoxPositionY, TextBoxSuggestionForeground);


            // Draw command
            d3dBase.DrawText(commandText, consoleFont, cursorBasePositionX, textBoxPositionY, TextBoxForeground);

            // Caret
            caretTick += 1;
            if (caretTick >= 35)
            {
                caretTick = 0;
                showCaret = !showCaret;
            }

            if (showCaret)
                d3dBase.DrawText("_", consoleFont, cursorBasePositionX + (caretPos * 8.3f), textBoxPositionY, TextBoxForeground);
        }

        public bool KeyPress(int code, IntPtr wParam, IntPtr lParam)
        {
            // If not keydown
            if (wParam != (IntPtr)0x100)
                return false;

            bool isUppercase = false;
            if (Control.ModifierKeys == Keys.Shift)
                isUppercase = true;

            char key = Convert.ToChar(code);
            if (isUppercase)
                key = char.ToUpper(key);
            else
                key = char.ToLower(key);


            if (code == Keys.NumPad8.GetHashCode())
                MoveWindowToTop();
            else if (code == Keys.NumPad4.GetHashCode())
                MoveWindowToLeft();
            else if (code == Keys.NumPad2.GetHashCode())
                MoveWindowToBottom();
            else if (code == Keys.NumPad6.GetHashCode())
                MoveWindowToRight();
            else if (code == Keys.NumPad9.GetHashCode())
                IncreaseWindowWidth();
            else if (code == Keys.NumPad7.GetHashCode())
                DecreaseWindowWidth();
            else if (code == Keys.NumPad0.GetHashCode())
                IncreaseWindowHeight();
            else if (code == Keys.NumPad1.GetHashCode())
                DecreaseWindowHeight();
            else if (char.IsLetter(key) || char.IsDigit(key) || key == Keys.Space.GetHashCode())
                AddToCommand(key);
            else if (code == Keys.Back.GetHashCode())
                BackKey();
            else if (code == Keys.Delete.GetHashCode())
                DeleteKey();
            else if (code == Keys.End.GetHashCode())
                MoveCaretToEnd();
            else if (code == Keys.Home.GetHashCode())
                MoveCaretToStart();
            else if (code == Keys.Down.GetHashCode())
                SetPreviousSuggestion();
            else if (code == Keys.Up.GetHashCode())
                SetNextSuggestion();
            else if (code == Keys.Left.GetHashCode())
                MoveCaretLeft();
            else if (code == Keys.Right.GetHashCode())
                MoveCaretRight();
            else if (code == Keys.Tab.GetHashCode())
                AcceptSuggestion();
            else if (code == Keys.PageUp.GetHashCode())
                SetNextCommandHistory();
            else if (code == Keys.PageDown.GetHashCode())
                SetPreviousCommandHistory();
            else if (code == Keys.Enter.GetHashCode())
                ProcessCommand();

            return true;
        }

        public void AddToLog(string text)
        {
            log += text + "\n";

            CalculateLogTextHeight();
        }

        private void CalculateLogTextHeight()
        {
            // Get text height
            Size2F logTextHeight = d3dBase.MeasureText(log, logFont);
            float logHeight = (Height - (TitleHeight + 5) - TextBoxHeight);

            if (logTextHeight.Height > logHeight)
            {
                // Get difference
                float heightDifference = logTextHeight.Height - logHeight;

                // Calculate how many lines this would be
                int lines = (int)Math.Ceiling(heightDifference / logLineHeight);

                // Remove that many lines so that the text can fit
                for (int i = 0; i < lines; i++)
                    log = log.Remove(0, log.IndexOf("\n") + 1);
            }
        }

        private void ShowHelp(string command = "")
        {
            string helpText = string.Empty;

            // Single Command
            if (command != "")
            {
                ConsoleCommand commandToShow = Commands.FirstOrDefault(c => c.Command == command);
                if (commandToShow != null)
                    helpText += string.Format("Command '{0}'\nDescirption: {1}\nSyntax: {2}", commandToShow.Command, commandToShow.Description, commandToShow.Syntax);
            }
            else
            {
                helpText = "Following commands are available:\n";

                // All
                foreach (ConsoleCommand c in Commands)
                    helpText += string.Format("{0}: {1}\n", c.Command, c.Description);

                helpText += "Please see the help page of the single command, for more information.";
            }


            AddToLog(helpText);
        }

        #region Default commands

        private void HideCommandExecuted(ConsoleCommandArgs e)
        {
            this.IsVisible = false;
        }

        private void ClearCommandExecuted(ConsoleCommandArgs e)
        {
            log = "-- Private Dayz TN Multihack Console -- \n";
        }

        #endregion

        #region Command

        private void ProcessCommand()
        {
            if (string.IsNullOrWhiteSpace(commandText))
            {
                ResetCommand();
                return;
            }

            AddCommandToHistory(commandText);

            // Split command and data
            string[] commandArray = commandText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string command = commandArray[0];
            string[] commandData = new string[commandArray.Length - 1];

            for (int i = 1; i < commandArray.Length; i++)
                commandData[i - 1] = commandArray[i];

            ResetCommand();

            bool commandFound = false;

            if (command == "help")
            {
                if (commandData.Length > 0)
                    ShowHelp(commandData[0]);
                else
                    ShowHelp();

                return;
            }

            foreach (ConsoleCommand receiver in Commands.Where(c => command == c.Command))
            {
                ConsoleCommand.OnCommandEventHandler r = receiver.Receiver;
                r(new ConsoleCommandArgs(commandData, receiver, AddToLog));
                commandFound = true;
            }

            if (!commandFound)
                AddToLog(string.Format("Command '{0}' not found!", command));
        }

        private void DeleteKey()
        {
            if (commandText.Length > 0 && caretPos >= 0 && caretPos < commandText.Length)
                RemoveFromCommand(caretPos, 1, false);
        }

        private void BackKey()
        {
            if (commandText.Length > 0 && caretPos > 0)
                RemoveFromCommand(caretPos - 1, 1, true);
        }

        private void ResetCommand()
        {
            commandText = string.Empty;
            caretPos = 0;

            GetSuggestion();
        }

        private void SetCommand(string text)
        {
            if (text.Length > 70)
                return;

            commandText = text;
            MoveCaretToEnd();
            GetSuggestion();
        }

        private void AddToCommand(char key)
        {
            if (commandText.Length +1 > 70)
                return;

            commandText = commandText.Insert(caretPos, key.ToString());
            caretPos++;

            GetSuggestion();
        }

        private void RemoveFromCommand(int index, int count, bool removeFromCaret)
        {
            if (commandText.Length > 0 && caretPos >= 0)
            {
                commandText = commandText.Remove(index, count);

                if(removeFromCaret)
                    caretPos--;

                GetSuggestion();
            }
        }

        #endregion

        #region Caret position

        private void MoveCaretLeft()
        {
            if (caretPos > 0)
                caretPos--;
        }

        private void MoveCaretRight()
        {
            if (caretPos + 1 <= commandText.Length)
                caretPos++;
        }

        private void MoveCaretToEnd()
        {
            caretPos = commandText.Length;
        }

        private void MoveCaretToStart()
        {
            caretPos = 0;
        }

        #endregion

        #region Suggestions

        private void AcceptSuggestion()
        {
            if (!string.IsNullOrWhiteSpace(currentSuggestion))
                SetCommand(currentSuggestion + " ");
        }

        private void GetSuggestion()
        {
            suggestions.Clear();

            if (commandText.Length < 1)
            {
                currentSuggestion = string.Empty;
                return;
            }


            // Check if full command is already written
            bool fullCommandWritten = commandText.Contains(" ");
            bool isHelpCommand = (commandText.Contains("help "));

            if (fullCommandWritten && !isHelpCommand)
            {
                // Get command that was written
                ConsoleCommand currentCommand = Commands.FirstOrDefault(c => commandText.ToLower().StartsWith(c.Command.ToLower()));

                // Get sub suggestions
                if (currentCommand != null && currentCommand.AutoCompleteCollection.Count > 0)
                {
                    string[] commandArray = commandText.Split(' ');
                    if (commandArray.Length > 1)
                    {
                        string command = commandArray[0];
                        string subCommand = commandArray[1];

                        List<string> subSuggestions = currentCommand.AutoCompleteCollection.Where(s => s.ToLower().StartsWith(subCommand.ToLower())).ToList();

                        foreach (string subSeggestion in subSuggestions)
                            suggestions.Add(currentCommand.Command + " " + subSeggestion);
                    }
                }
            }
            else if(isHelpCommand)
            {
                string helpParamter = commandText.Remove(0, 5);
                List<ConsoleCommand> commandSuggestions = Commands.Where(c => c.Command.StartsWith(helpParamter)).ToList();

                foreach (ConsoleCommand command in commandSuggestions)
                    suggestions.Add("help " + command.Command);
            }
            else
            {
                List<ConsoleCommand> commandSuggestions = Commands.Where(c => c.Command.StartsWith(commandText)).ToList();

                foreach (ConsoleCommand command in commandSuggestions)
                    suggestions.Add(command.Command);
            }


            if (suggestions.Count > 0)
            {
                currentSuggestion = suggestions[0];
                currentSuggestionIndex = 0;
            }
            else
            {
                currentSuggestion = string.Empty;
            }
        }

        private void SetNextSuggestion()
        {
            if (suggestions.Count <= 0)
                return;

            if (suggestions.Count <= currentSuggestionIndex + 1)
                currentSuggestionIndex = 0;
            else
                currentSuggestionIndex++;

            currentSuggestion = suggestions[currentSuggestionIndex];
        }

        private void SetPreviousSuggestion()
        {
            if (suggestions.Count <= 0)
                return;

            if (currentSuggestionIndex - 1 < 0)
                currentSuggestionIndex = suggestions.Count -1;
            else
                currentSuggestionIndex--;

            currentSuggestion = suggestions[currentSuggestionIndex];
        }

        #endregion

        #region Command history

        private void AddCommandToHistory(string command)
        {
            if (commandHistory.Count >= 30)
                commandHistory.RemoveAt(0);

            commandHistory.Add(new HistoryCommand(DateTime.Now, command));
            commandHistory = commandHistory.OrderByDescending(ch => ch.AddTime).ToList();

            currentCommandHistoryIndex = -1;
        }

        private void SetNextCommandHistory()
        {
            if (commandHistory.Count <= 0)
                return;

            if (commandHistory.Count > currentCommandHistoryIndex + 1)
                currentCommandHistoryIndex++;

            SetCommand(commandHistory[currentCommandHistoryIndex].Command);
        }

        private void SetPreviousCommandHistory()
        {
            if (commandHistory.Count <= 0)
                return;

            if (currentCommandHistoryIndex - 1 == -1)
            {
                currentCommandHistoryIndex--;
                SetCommand("");
            }
            else if(currentCommandHistoryIndex - 1 >= -1)
            {
                currentCommandHistoryIndex--;
                SetCommand(commandHistory[currentCommandHistoryIndex].Command);
            }
        }

        #endregion

        #region Position & Size

        private void IncreaseWindowWidth()
        {
            float newWindowWidth = Width + WindowSizeModifier;
            if (PositionX + newWindowWidth < d3dBase.Overlay.Width)
                Width = newWindowWidth;
        }

        private void DecreaseWindowWidth()
        {
            float newWindowWidth = Width - WindowSizeModifier;
            if (newWindowWidth >= MinWidth)
                Width = newWindowWidth;
        }

        private void IncreaseWindowHeight()
        {
            float newWindowHeight = Height + WindowSizeModifier;
            if (PositionY + newWindowHeight < d3dBase.Overlay.Height)
                Height = newWindowHeight;
        }

        private void DecreaseWindowHeight()
        {
            float newWindowHeight = Height - WindowSizeModifier;
            if (newWindowHeight >= MinHeight)
                Height = newWindowHeight;

            CalculateLogTextHeight();
        }

        private void MoveWindowToLeft()
        {
            float newPositionX = PositionX - WindowPositionModifier;
            if (newPositionX > 0)
                PositionX = newPositionX;
        }

        private void MoveWindowToRight()
        {
            float newPositionX = PositionX + WindowPositionModifier;
            if (newPositionX + Width < d3dBase.Overlay.Width)
                PositionX = newPositionX;
        }

        private void MoveWindowToBottom()
        {
            float newPositionY = PositionY + WindowPositionModifier;
            if (newPositionY + Height < d3dBase.Overlay.Height)
                PositionY = newPositionY;
        }

        private void MoveWindowToTop()
        {
            float newPositionY = PositionY - WindowPositionModifier;
            if (newPositionY > 0)
                PositionY = newPositionY;
        }

        #endregion

        #endregion
    }
}
