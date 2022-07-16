using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InputForms;

namespace Car_Dealership
{
    public partial class Codec : Form
    {
        InputForm form;

        Label labelHelp;

        Label labelStatus;

        CancellationTokenSource cancellationTokenSource;

        CancellationToken cancellationToken;

        /// <summary>
        /// Application encryption level if zero then no encryption. 
        /// Must be greater than -1
        /// </summary>
        public static int LevelOfAppEncryption = 0;

        /// <summary>
        /// Application setting the save type. 
        /// If true, the format is text with a ".csv" extension. 
        /// If false the format is binary with ".dat" extension.
        /// </summary>
        public static bool TextTrue_BinFalse = true;

        int LevelOfSource = 0;

        int LevelOfTarget = 0;

        string SourceFilename = String.Empty;

        string SourcExtension = String.Empty;

        string TargetExtension = String.Empty;

        const string DropDownMenuAllTables = "Alle Tabellen";

        const string DropDownMenuCostumer = "Costumer";

        const string DropDownMenuCar = "Car";

        const string DropDownMenuSale = "Sale";

        const string DropDownMenuSalesperson = "Salesperson";

        const string DropDownMenuSalesperson_Secret_Data = "Salesperson_Secret_Data";

        const string DropDownMenuUsernameAndPasswords = "UsernameAndPasswords";

        const string DropDownMenuLogin = "Login";

        const string DropDownMenuExtension1 = ".csv";

        const string DropDownMenuExtension2 = ".dat";

        public const string TheTableCompletedMessage = "Tabelle ist fertiggestellt";

        const string Help =
                         "Mehrstufige Ver- und Entschlüsselung von Tabellen speichern in Text- oder Binärformaten " +
                       "\n.csv oder .dat Erweiterung " +
                       "\n" +
                       "\nBevor Sie beginnen, vergewissern Sie sich, " +
                       "\ndass sich die von Ihnen gewählte Quelldatei im Ordner befindet" +
                       "\n" +
                       "\nDas fertige Dokument finden Sie in der Anwendungsordner mit Datum und Uhrzeit Titel" +
                       "\n" +
                       "\nAnwendungsverschlüsselungsstufe und Format" +
                       "\nEntfernen Sie das Datum aus dem Titel der neu erstellten neuen Verschlüsselungsstufentabellen" +
                       "\nNur ändern, wenn die neuen Verschlüsselungsstufentabellen verfügbar sind" +
                       "\nin \"Tabellename Verschlüsselungsstufe level.Erweiterung\" Format" +
                       "\n" +
                       "\nVon der Anwendung zur Laufzeit verwendete Tabellen" +
                       "\nDer Verschlüsselungsgrad und die Erweiterung der Tabellen können sich nicht voneinander unterscheiden" +
                       "\nDie Einstellung der Dateierweiterung und der Verschlüsselungsstufe gelten für alle Tabellen" +
                       //  "\n - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - " +
                       "\n";
        const string Status =
                         "Speichern läuft " +
                       //  "\n - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - " +
                       "\n";


        private void Start()
        {
            form = new InputForm(this);
            form.Add("Source filename", new InputSelect("Quelltabellename:", DropDownMenuTables()).SetSize(200))
                .Add("Source level of encryption", new InputSelect("Quellebene der Verschlüsselung:", DropDownMenuNumbers()).SetSize(200))
                .Add("Source file extension", new InputSelect("Quelldateierweiterung:", DropDownMenuExtensions()).SelectIndex(SetOfSelectedIndex()).SetSize(200))
                .Add("Target level of encryption", new InputSelect("Zielebene der Verschlüsselung:", DropDownMenuNumbers()).SelectIndex(LevelOfAppEncryption).SetSize(130))
                .Add("Target file extension", new InputSelect("Zieldateierweiterung:", DropDownMenuExtensions()).SetSize(130))
                .Add("Level Of App Encryption", new InputSelect("Appsverschlüsselungsstufe:", DropDownMenuNumbers()).SelectIndex(LevelOfAppEncryption).SetSize(70))
                .Add("Text or Binary", new InputSelect("Appsverschlüsselungsstufe:", DropDownMenuExtensions()).SelectIndex(SetOfSelectedIndex()).SetSize(70))
                .MoveTo(10, 10)
                .SetButtonZero("Tabelle speichern")
                .SetButtonFirst("Einstellen App", Visible = true)
                .SetButtonSecond("Hilfe", Visible = true)
                .SetButtonThird("Menu", Visible = true)
                .OnSubmitZero(() =>
                {
                    StorageInVariables();

                    if (labelHelp.Visible == true) labelHelp.Visible = false;

                    CallSave();
                })
                .OnSubmitFirst(() =>
                {
                    int setLevel = Convert.ToInt32(form["Level Of App Encryption"]);

                    string extensionFormat = form["Text or Binary"];

                    SetAppEncryptionAndExtension(setLevel, extensionFormat);

                    MessageBox.Show($"Die neue Stufe ({setLevel}) und format ({extensionFormat}) ist eingestellt");
                })
                .OnSubmitSecond(() =>
                {
                    labelHelp.Visible = true;
                    labelHelp.Text = Help;
                })
                .OnSubmitThird(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private static string[] DropDownMenuTables()
        {
            string[] MenuTables = new string[8]
            {
                DropDownMenuAllTables,
                DropDownMenuCostumer,
                DropDownMenuCar,
                DropDownMenuSale,
                DropDownMenuSalesperson,
                DropDownMenuSalesperson_Secret_Data,
                DropDownMenuUsernameAndPasswords,
                DropDownMenuLogin
            };

            return MenuTables;
        }

        private static string[] DropDownMenuNumbers()
        {
            string[] MenuNumbers = new string[10];
            for (int i = 0; i < MenuNumbers.Length; i++)
            {
                MenuNumbers[i] = i.ToString();
            }
            return MenuNumbers;
        }

        private static string[] DropDownMenuExtensions()
        {
            string[] MenuExtensions = new string[]
            {
                DropDownMenuExtension1,
                DropDownMenuExtension2
            };
            return MenuExtensions;
        }

        /// <summary>
        /// The async method can't access it, so store it in variables
        /// </summary>
        private void StorageInVariables()
        {
            LevelOfSource = Convert.ToInt32(form["Source level of encryption"]);

            LevelOfTarget = Convert.ToInt32(form["Target level of encryption"]);

            SourceFilename = form["Source filename"];

            SourcExtension = form["Source file extension"];

            TargetExtension = form["Target file extension"];
        }

        private async void CallSave()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            Task.Run(Waiting);
            await SaveTable();
            cancellationTokenSource.Cancel();
            MessageBox.Show(TheTableCompletedMessage);
            cancellationTokenSource = null;
        }

        private Task Waiting()
        {
            return Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    labelStatus.Invoke(() => labelStatus.Text = Status);
                    Thread.Sleep(500);
                    labelStatus.Invoke(new Action(() => labelStatus.Text = String.Empty));
                    Thread.Sleep(500);
                }
            });
        }
      
        private Task SaveTable()
        {
            return Task.Factory.StartNew(() =>
            {
                switch (SourceFilename)
                {
                    case DropDownMenuAllTables:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Costumers.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    Cars.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    Sales.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    SalesPersons.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    SalesPersons_Secret_Data.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    Usernames_And_Passwords.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    Logins.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Costumers.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    Cars.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    Sales.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    SalesPersons.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    SalesPersons_Secret_Data.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    Usernames_And_Passwords.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    Logins.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Costumers.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    Cars.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    Sales.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    SalesPersons.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    SalesPersons_Secret_Data.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    Usernames_And_Passwords.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    Logins.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Costumers.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    Cars.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    Sales.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    SalesPersons.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    SalesPersons_Secret_Data.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    Usernames_And_Passwords.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    Logins.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuCostumer:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Costumers.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Costumers.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Costumers.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Costumers.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuCar:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Cars.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Cars.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Cars.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Cars.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuSale:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Sales.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Sales.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Sales.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Sales.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuSalesperson:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    SalesPersons.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    SalesPersons.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    SalesPersons.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    SalesPersons.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuSalesperson_Secret_Data:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    SalesPersons_Secret_Data.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    SalesPersons_Secret_Data.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    SalesPersons_Secret_Data.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    SalesPersons_Secret_Data.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuUsernameAndPasswords:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Usernames_And_Passwords.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Usernames_And_Passwords.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Usernames_And_Passwords.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Usernames_And_Passwords.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case DropDownMenuLogin:
                        {
                            switch (SourcExtension)
                            {
                                case DropDownMenuExtension1:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Logins.EncryptionTextToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Logins.EncryptionTextToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                case DropDownMenuExtension2:
                                    {
                                        switch (TargetExtension)
                                        {
                                            case DropDownMenuExtension1:
                                                {
                                                    Logins.EncryptionBinToText(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            case DropDownMenuExtension2:
                                                {
                                                    Logins.EncryptionBinToBin(LevelOfSource, LevelOfTarget);
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    default:
                        break;
                }
            });
        }


        private void CreateLabels()
        {
            labelHelp = new Label();

            labelHelp.Visible = true;

            labelHelp.Location = new Point(280, 80);

            labelHelp.Size = new Size(25, 245);

            labelHelp.Font = new Font("sans-serif", 11f);

            labelHelp.AutoSize = true;

            this.Controls.Add(labelHelp);


            labelStatus = new Label();

            labelStatus.Visible = true;

            labelStatus.Location = new Point(400, 300);

            labelStatus.Size = new Size(25, 245);

            labelStatus.Font = new Font("sans-serif", 25f);

            labelStatus.AutoSize = true;

            this.Controls.Add(labelStatus);
        }

        public static void CreateAppEncryptionAndExtension()
        {
            // Level
            string storedLevelOfAppEncryption = null;

            if (File.Exists("appencrypt"))
                storedLevelOfAppEncryption = File.ReadAllText("appencrypt");

            if (storedLevelOfAppEncryption == null || storedLevelOfAppEncryption.Length < 0)
                storedLevelOfAppEncryption = "0";

            Codec.LevelOfAppEncryption = Convert.ToInt32(storedLevelOfAppEncryption);

            File.WriteAllText("appencrypt", LevelOfAppEncryption.ToString());

            // Extension
            string storedLevelOfAppExtension = null;

            if (File.Exists("appextension"))
                storedLevelOfAppExtension = File.ReadAllText("appextension");

            if (storedLevelOfAppExtension == null || storedLevelOfAppExtension.Length < 0)
                storedLevelOfAppExtension = "true";

            Codec.TextTrue_BinFalse = Convert.ToBoolean(storedLevelOfAppExtension);

            File.WriteAllText("appextension", TextTrue_BinFalse.ToString());
        }

        static void SetAppEncryptionAndExtension(int setLevel, string extensionFormat)
        {
            // Level
            File.WriteAllText("appencrypt", setLevel.ToString());

            LevelOfAppEncryption = Convert.ToInt32(setLevel);

            // Extension
            bool setExtension = false;

            if (extensionFormat == (DropDownMenuExtensions()[0]))
                setExtension = true;
            else
                setExtension = false;

            File.WriteAllText("appextension", setExtension.ToString());

            TextTrue_BinFalse = Convert.ToBoolean(setExtension);
        }

        static int SetOfSelectedIndex()
        {
            if (TextTrue_BinFalse)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public Codec()
        {
            InitializeComponent();

            CreateLabels();

            Start();
        }

    }
}
