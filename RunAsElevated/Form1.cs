using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RunAsElevated
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Empêcher le redimensionnement du formulaire
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Lire les arguments de la ligne de commande
            string[] args = Environment.GetCommandLineArgs();
            ProcessCommandLineArgs(args);
        }
        private void ProcessCommandLineArgs(string[] args)
        {
            // Ne rien faire si aucun argument n'est fourni
            if (args.Length < 3) return;

            try
            {
                string argumentType = args[1].ToLower();
                string filePath = args[2];

                // Vérifier si l'argument correspond à -r ou -rpath
                if (argumentType == "-rpath" || argumentType == "-r")
                {
                    // Vérifier si le fichier existe
                    if (System.IO.File.Exists(filePath))
                    {
                        RunAsAdministrator(filePath);
                        Environment.Exit(0);  // Force la fermeture de l'application
                    }
                    else
                    {
                        MessageBox.Show("Le fichier spécifié n'existe pas : " + filePath, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);  // Force la fermeture de l'application
                    }
                }
                else
                {
                    MessageBox.Show($"Argument non reconnu : {argumentType}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);  // Force la fermeture de l'application
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du traitement des arguments : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);  // Force la fermeture de l'application
            }
        }



        // Méthode appelée lors du clic sur le bouton "Parcourir"
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                openFileDialog.Title = "Sélectionnez un fichier .exe";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void RunAsAdministrator(string filePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    Verb = "runas", // Exécute en tant qu'administrateur
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exécution : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode appelée lors du clic sur le bouton "Exécuter"
        private void btnRun_Click_1(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Veuillez sélectionner un fichier .exe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (System.IO.File.Exists(filePath))
            {
                RunAsAdministrator(filePath);
            }
            else
            {
                MessageBox.Show("Le fichier spécifié n'existe pas : " + filePath, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
