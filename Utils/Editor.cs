﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PygmyModManager.Utils
{
    public partial class Editor : Form
    {
        public Editor(string path = "")
        {
            InitializeComponent();

            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;

            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveChangesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;

            OpenFile(path);
        }

        void OpenFile(string path = "")
        {
            string filename = "";

            if (path == "")
            {
                DialogResult userChoseFile = openFileDialog1.ShowDialog();

                if (!(userChoseFile == DialogResult.OK)) return;
                filename = openFileDialog1.FileName;
            } else
            {
                filename = path;
            }

                this.Text = "File Editor - " + Path.GetFileName(filename);
            fileLocationBox.Text = filename;
            fileContentBox.Text = File.ReadAllText(filename);
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileContentBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileContentBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileContentBox.Paste();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";

            if (fileLocationBox.Text == "")
            {
                DialogResult doSaveIt = saveFileDialog1.ShowDialog();

                if (doSaveIt == DialogResult.OK)
                    path = saveFileDialog1.FileName;
                else
                    return; // cancel
            }
            else
            {
                path = fileLocationBox.Text;
            }

            File.WriteAllText(path, fileContentBox.Text); // yes, it has multiline support, don't worry :)
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
    }
}
