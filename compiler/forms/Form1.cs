﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Schema;

namespace compiler
{
    public partial class Form1 : Form
    {
        private string pattern;
        public Form1()
        {
            InitializeComponent();
            StatusTime();
            this.DragDrop += Form1_DragDrop;
            this.DragEnter += Form1_DragEnter;
            KeyPreview = true;
            KeyDown += Form1_KeyDown;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                CreateFile();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                SaveFile();
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                OpenFile();

            }
        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if (Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        AddPage(file);
                    }
                    else
                    {
                        MessageBox.Show("Формат файла не поддерживается!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void UpdateLineNumbers(RichTextBox richTextBox, RichTextBox lineNumbersBox)
        {
            if (richTextBox == null || lineNumbersBox == null) return;

            int firstLine = richTextBox.GetLineFromCharIndex(0);  // Получаем первую строку
            int lastLine = richTextBox.GetLineFromCharIndex(richTextBox.TextLength);  // Получаем последнюю строку

            lineNumbersBox.Clear();
            for (int i = firstLine; i <= lastLine; i++)
            {
                // Печатаем номер строки
                lineNumbersBox.AppendText((i + 1) + Environment.NewLine);
            }
        }


        private void CreateFile()
        {
            saveFileDialog1.Filter = "Тестовый файл (*.txt)|*.txt";
            saveFileDialog1.Title = "Создание файла";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;

                try
                {
                    File.WriteAllText(filename, string.Empty);
                    AddPage(filename);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании файла: " + ex.Message,
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFile()
        {
            openFileDialog1.Filter = "Тестовый файл (*.txt)|*.txt";
            openFileDialog1.Title = "Открытие файла";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                AddPage(filename);
            }
        }



        private void AddPage(string filename)
        {
            TabPage tabPage = new TabPage(Path.GetFileName(filename));
            tabPage.Tag = filename;

            Panel panel = new Panel { Dock = DockStyle.Fill };

            int fontSize = Properties.Settings.Default.FontSize;


            RichTextBox lineNumbersBox = new RichTextBox
            {
                ReadOnly = true,
                Width = 40,
                Dock = DockStyle.Left,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Consolas", fontSize),
                ScrollBars = RichTextBoxScrollBars.None,
                Tag = "LineNumbers"
            };


            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Text = File.ReadAllText(filename),
                Font = new Font("Consolas", fontSize),
                Tag = "Editor",
            };

            richTextBox.VScroll += (s, e) => UpdateLineNumbers(richTextBox, lineNumbersBox);
            richTextBox.TextChanged += (s, e) => UpdateLineNumbers(richTextBox, lineNumbersBox);
            richTextBox.SelectionChanged += (s, e) => UpdateLineNumbers(richTextBox, lineNumbersBox);
            richTextBox.MouseWheel += RichTextBox_MouseWheel;
            lineNumbersBox.MouseWheel += RichTextBox_MouseWheel;

            panel.Controls.Add(richTextBox);
            panel.Controls.Add(lineNumbersBox);

            tabPage.Controls.Add(panel);
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;


            UpdateLineNumbers(richTextBox, lineNumbersBox);
        }

        private void RichTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }


        private void Undo()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null && richTextBox.CanUndo)
                {
                    richTextBox.Undo();
                }
            }
        }

        private void Redo()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null && richTextBox.CanRedo)
                {
                    richTextBox.Redo();
                }
            }
        }

        private void Copy()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null)
                {
                    richTextBox.Copy();
                }
            }
        }

        private void Cut()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null)
                {
                    richTextBox.Cut();
                }
            }
        }

        private void Paste()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null)
                {
                    richTextBox.Paste();
                }
            }
        }

        private void SaveFile()
        {
            if (tabControl1.SelectedTab != null)
            {
                string filename = tabControl1.SelectedTab.Tag as string;

                if (!string.IsNullOrEmpty(filename))
                {
                    RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;

                    try
                    {
                        string path = Path.GetFullPath(filename);
                        File.WriteAllText(path, richTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Файл не был ранее сохранён. Используйте 'Сохранить как...'",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void SaveAsFile()
        {
            if (tabControl1.SelectedTab != null)
            {
                saveFileDialog1.Filter = "Текстовый файл (*.txt)|*.txt";
                saveFileDialog1.Title = "Сохранить как...";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;

                    if (richTextBox != null)
                    {
                        try
                        {
                            File.WriteAllText(filename, richTextBox.Text);

                            tabControl1.SelectedTab.Tag = filename;

                            tabControl1.SelectedTab.Text = Path.GetFileName(filename);

                            MessageBox.Show("Файл успешно сохранён!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void Delete()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null && richTextBox.SelectedText.Length > 0)
                {
                    richTextBox.SelectedText = "";
                }
            }
        }

        private void SelectAll()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null)
                {
                    richTextBox.SelectAll();
                }
            }
        }


        private void Exit()
        {
            if (tabControl1.SelectedTab != null)
            {
                var result = MessageBox.Show($"Сохранить изменения в {tabControl1.SelectedTab.Text} ?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                this.Close();
            }
        }
        private void createToolStripButton_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsFile();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void backlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;
                if (richTextBox != null && richTextBox.SelectionLength > 0)
                {

                    richTextBox.SelectionColor =
                richTextBox.SelectionColor == Color.Red ? Color.Black : Color.Red;
                }
            }

        }

        private void StatusTime()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
            {
                Interval = 100,
            };
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            dateToolStripStatusLabel.Text = DateTime.Now.ToShortDateString();
            timeToolStripStatusLabel.Text = DateTime.Now.ToShortTimeString();
        }

        private void ApplyFontSizeToAll()
        {
            int fontSize = Properties.Settings.Default.FontSize;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0 && tab.Controls[0] is Panel panel)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control is RichTextBox richTextBox)
                        {
                            richTextBox.Font = new Font("Consolas", fontSize);
                        }
                    }
                }
            }
        }
        public void ApplySettings()
        {
            ApplyFontSizeToAll();
        }


        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsFormcs settingsFormcs = new SettingsFormcs();
            settingsFormcs.ShowDialog();
            ApplySettings();
        }

        private void About()
        {
            AboutFormcs aboutFormcs = new AboutFormcs();
            aboutFormcs.Show();
        }
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            About();
        }

        private void OpenHTML(string htmlText)
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "example.html");
            File.WriteAllText(tempFile, htmlText, Encoding.UTF8);

            Process.Start(new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            });
        }
        private void Help()
        {
            OpenHTML(Properties.Resources.справка1);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Help();
            Dictionary<int, string> My = new Dictionary<int, string>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Сохранить перед выходом все изменения в файлах?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (var tabPage in tabControl1.TabPages)
                {
                    SaveFile();
                }
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {

                dataGridView1.Columns.Clear();
                dataGridView2.Columns.Clear();
                RichTextBox richTextBox = ((Panel)tabControl1.SelectedTab.Controls[0]).Controls[0] as RichTextBox;

                Parser parser = new Parser(richTextBox.Text);
                parser.Parse();

                Lexer lexer = new Lexer(richTextBox.Text);
                lexer.Analyze();

                dataGridView1.DataSource = lexer.Tokens.Select(ee => new
                {
                    Тип = ee.Type.ToString(),
                    Значение = ee.Value,
                    Местоположение = $"({ee.Position.Item1 + 1},{ee.Position.Item2 + 1})"
                }).ToList();

                List<Error> errors = parser.Errors;
                errors.AddRange(parser.Lexer.Errors);
                errors = errors.OrderBy(x => x.Position.start).ToList();
                dataGridView2.DataSource = errors.Select(ee => new
                {
                    Сообщение = ee.Message,
                    НеожиданноеЗначение = ee.Fragment,
                    Местоположение = $"({ee.Position.start + 1},{ee.Position.end})"
                }).ToList();
                richTextBox.SelectAll();
                richTextBox.SelectionBackColor = richTextBox.BackColor;

                foreach (var error in errors)
                {
                    richTextBox.Select(error.Position.start, error.Position.end - error.Position.start);
                    richTextBox.SelectionBackColor = Color.Red;
                }
                richTextBox.Select(0, 0);
                richTextBox.SelectionBackColor = richTextBox.BackColor;
                richTextBox1.Clear();
                richTextBox1.Text = string.Join(") --- (", parser.Log);
            }
        }


        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.постановка_задачи);
        }

        private void grammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.Грамматика);
        }

        private void classificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.Классификация_грамматики);
        }

        private void methodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHtmlWithEmbeddedImages();
        }

        private void exampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.тест);
        }

        private void literatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.Список_литературы);
        }
        string EmbedImage(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                string base64 = Convert.ToBase64String(ms.ToArray());
                return $"data:image/png;base64,{base64}";
            }
        }

        void OpenHtmlWithEmbeddedImages()
        {
            string html = Properties.Resources.Метод_анализа;

            html = html.Replace("Диаграмма состояния сканера.png", EmbedImage(Properties.Resources.Диаграмма_состояния_сканера));
            html = html.Replace("граф конечного автомата.png", EmbedImage(Properties.Resources.граф_конечного_автомата));

            string tempFile = Path.Combine(Path.GetTempPath(), "метод_анализа.html");
            File.WriteAllText(tempFile, html, Encoding.UTF8);

            Process.Start(new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            });
        }
        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHTML(Properties.Resources.Листинг_программы);
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pattern = @"\b[A-Fa-f0-9]{6}\b";
            tabPage1.Text = $"Совпадения {hexToolStripMenuItem.Text}";
        }

        private void masterCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pattern = @"\b5[1-5][0-9]{2}(-[0-9]{4}){3}\b";
            tabPage1.Text = $"Совпадения {masterCardToolStripMenuItem.Text}";
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabPage1.Text = $"Совпадения {passwordToolStripMenuItem.Text}";
            pattern = @"(?=.*[А-Я])(?=.*[а-я])(?=.*[0-9])(?=.*[#?!|@/$%^&*\-_])[А-Яа-я0-9#?!|@$%^&*\-_]{8,}";

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }
    }
}
