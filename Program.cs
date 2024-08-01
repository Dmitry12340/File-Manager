using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace File_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\Users\kubte\OneDrive\Рабочий стол\1.rtf";
            //using (FileStream fileStream = new FileStream(path, FileMode.Append))
            //{
            //    using (StreamWriter streamWriter = new StreamWriter(fileStream))
            //    {
            //        streamWriter.WriteLine("skjdghjk");
            //        streamWriter.WriteLine("skjdghjk");
            //        streamWriter.WriteLine("skjdghjk");
            //        streamWriter.Dispose();
            //    }
            //    fileStream.Dispose();
            //}


            //Пример с получение всех папок и файлов дерриктории
            //string dirName = @"C:\";
            //// если папка существует
            //if (Directory.Exists(dirName))
            //{
            //    Console.WriteLine("Подкаталоги:");
            //    string[] dirs = Directory.GetDirectories(dirName);
            //    foreach (string s in dirs)
            //    {
            //        Console.WriteLine(s);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine("Файлы:");
            //    string[] files = Directory.GetFiles(dirName);
            //    foreach (string s in files)
            //    {
            //        Console.WriteLine(s);
            //    }
            //}

            var elems = new[] { new Element("Google") { Command = GoogleCommandHandler},
                new Element("One"),
                new Element("Test"),
            new Element("    \nExit\n    "){ Command = ExitCommandHandler } };
            Menu menu = new Menu(elems);
            while (true)
            {
                menu.Draw();
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.SelectPrev();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.SelectNext();
                        break;
                    case ConsoleKey.Enter:
                        menu.ExecuteSelected();
                        break;
                    default: return;
                }
            }
        }
        private static void GoogleCommandHandler()
        {
            Process.Start("http://www.google.ru/");
        }

        private static void ExitCommandHandler()
        {
            Environment.Exit(0);
        }
    }

    delegate void CommandHandler();
    class Menu
    {
        public Element[] Elements { get; set; }
        public int Index { get; set; }

        public Menu(Element[] elems)
        {
            this.Index = 0;
            this.Elements = elems;
            Elements[Index].IsSelected = true;
        }

        public void Draw()
        {
            Console.Clear();
            foreach (var element in Elements)
            {
                element.Print();
            }
        }

        public void SelectNext()
        {
            if (Index == Elements.Length - 1) return;
            Elements[Index].IsSelected = false;
            Elements[++Index].IsSelected = true;
        }

        public void SelectPrev()
        {
            if (Index == 0) return;
            Elements[Index].IsSelected = false;
            Elements[--Index].IsSelected = true;
        }

        public void ExecuteSelected()
        {
            Elements[Index].Execute();
        }
    }

    class Element
    {
        public string Text { get; set; }
        public ConsoleColor SelectedForeColor { get; set; }
        public ConsoleColor SelectedBackColor { get; set; }
        public bool IsSelected { get; set; }
        public CommandHandler Command;

        public Element(string text)
        {
            this.Text = text;
            this.SelectedForeColor = ConsoleColor.Black;
            this.SelectedBackColor = ConsoleColor.Gray;
            this.IsSelected = false;
        }

        public void Print()
        {
            if (this.IsSelected)
            {
                Console.BackgroundColor = this.SelectedBackColor;
                Console.ForegroundColor = this.SelectedForeColor;
            }
            Console.WriteLine(this.Text);
            Console.ResetColor();
        }

        public void Execute()
        {
            if (Command == null) return;
            Command.Invoke();
        }
    }
}
