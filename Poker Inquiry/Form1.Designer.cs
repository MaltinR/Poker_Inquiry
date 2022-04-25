
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Poker_Inquiry
{
    partial class Form1
    {
        enum tab
        {
            Pre,
            Flop,
            Turn
        }

        //Global value
        tab curTab = tab.Pre;
        Preflop preflop;
        Panel panel;
        PanelRandom panRand;

        Button TabPreflop;
        Button TabFlop;
        Button TabTurn;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 700);
            this.Name = "Poker Inquiry by MartinL";
            this.Text = "Poker Inquiry by MartinL";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

            panel = new Panel();
            Point locPanel = new Point(0, 25);
            panel.Location = locPanel;
            panel.AutoSize = false;
            panel.Width = 320;
            panel.Height = 475;
            this.Controls.Add(panel);

            preflop = new Preflop(this);

            TabPreflop = new Button();
            TabPreflop.Location = new Point(0, 0);
            TabPreflop.Text = "Preflop";
            this.Controls.Add(TabPreflop);

            TabFlop = new Button();
            TabFlop.Location = new Point(75, 0);
            TabFlop.Text = "Flop";
            //this.Controls.Add(TabFlop);

            TabTurn = new Button();
            TabTurn.Location = new Point(150, 0);
            TabTurn.Text = "Turn";
            //this.Controls.Add(TabTurn);

            panRand = new PanelRandom(this);
            this.Controls.Add(panRand.GetPanel());
        }
        public class PanelRandom
        {
            Panel panelRand;
            Form1 form;
            Button butGenerate;

            Random random;

            Panel[] panelResults = new Panel[2];
            Label[] textResults = new Label[2];
            public PanelRandom(Form1 _form)
            {
                form = _form;

                panelRand = new Panel();
                panelRand.Location = new Point(0, 500);
                panelRand.Width = 320;
                panelRand.Height = 200;

                butGenerate = new Button();
                butGenerate.AutoSize = false;
                butGenerate.Location = new Point(280, 0);
                butGenerate.Width = 40;
                butGenerate.Height = 200;
                butGenerate.Text = "?";
                panelRand.Controls.Add(butGenerate);
                butGenerate.Click += new EventHandler(Random);

                for (int i = 0;i < 2;i++)
                {
                    panelResults[i] = new Panel();
                    panelResults[i].AutoSize = false;
                    panelResults[i].Location = new Point(0, 50+i*100);
                    panelResults[i].BackColor = Color.Green;
                    panelResults[i].Width = 280;
                    panelResults[i].Height = 50;

                    textResults[i] = new Label();
                    textResults[i].AutoSize = false;
                    textResults[i].Location = new Point(0, i * 100);
                    textResults[i].Width = 280;
                    textResults[i].Height = 50;

                    panelRand.Controls.Add(panelResults[i]);
                    panelRand.Controls.Add(textResults[i]);
                }

                random = new Random();
                Random();
            }
            void Random(object sender, EventArgs e)
            {
                Random();
            }

            public void Random()
            {
                int tempRand;
                for (int i = 0; i < 2; i++)
                {
                    tempRand = random.Next(100);
                    panelResults[i].Width = (int) ((double)tempRand/100.0*280.0);
                    textResults[i].Text = tempRand.ToString() + "%";
                }
            }

            public Panel GetPanel()
            {
                return panelRand;
            }
        }

        public class TabPage
        {
            public virtual void Load()
            {

            }

            //Clear the elements
            public virtual void Clear()
            {

            }
        }

        public class Preflop : TabPage
        {
            //setting
            Form1 form;

            Table table = Table.none;
            Position posHero = Position.none;
            Action action = Action.none;
            Position posVillain = Position.none;
            int chip = 0;
            string filePic;
            PictureBox pic;//Maybe other type

            //UI
            Button[] butList = new Button[9];

            Button butSelectedTable;
            Button butSelectedHero;
            Button butSelectedAction;
            Button butSelectedFourth;
            Button butSelectedFifth;

            Label labOption;



            public Preflop(Form1 _form)
            {
                form = _form;

                for(int i = 0;i < 9;i++)
                {
                    butList[i] = new Button();
                    butList[i].Click += new EventHandler(ButtonOnClick);
                }

                butSelectedTable = new Button();
                butSelectedTable.Click += new EventHandler(ButtonOnClick);
                butSelectedHero = new Button();
                butSelectedHero.Click += new EventHandler(ButtonOnClick);
                butSelectedAction = new Button();
                butSelectedAction.Click += new EventHandler(ButtonOnClick);
                butSelectedFourth = new Button();
                butSelectedFourth.Click += new EventHandler(ButtonOnClick);
                butSelectedFifth = new Button();
                butSelectedFifth.Click += new EventHandler(ButtonOnClick);

                labOption = new Label();

                pic = new PictureBox();
                pic.Location = new Point(0, 75);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                //pic.Height = 200;

                Load();
            }

            public enum Table
            {
                none,
                MTT,
                Cash
            }

            public enum Action
            {
                none,
                FI,//First in
                Def,//Defend open
                V3B,//vs 3b
                V4B,
                RVP,//R vs Push !BB
                LVR,// Limp vs Raise
                LVP,//Limp vs Push (SB Only)
                VOP,//vs open push
                VL,//vs limp
                VLR,//vs limp/raise
                VLP//vs limp/push
            }

            public enum Position
            {
                none,
                BB,
                SB,
                BN,
                CO,
                HJ,
                LJ,
                UTG2,
                UTG1,
                UTG
            }

            public override void Load()
            {
                form.panel.Controls.Clear();

                if (table == Table.none)
                {
                    LoadButton();
                }
                else if(posHero == Position.none)
                {
                    LoadButton(table);
                }
                else if(action == Action.none)
                {
                    LoadButton(table, posHero);
                }
                else if (posVillain == Position.none)
                {
                    if (chip == 0)
                    {
                        LoadButton(table, posHero, action);
                    }
                    else
                    {
                        LoadButton(table, posHero, action, chip);
                    }
                }
                else if (chip == 0)
                {
                    LoadButton(table, posHero, action, posVillain);
                }
                else
                {
                    LoadButton(table, posHero, action, posVillain, chip);
                }

                LoadPic();
            }

            void LoadPic()
            {
                form.panel.Controls.Add(pic);
            }
            //Table
            void LoadButton()
            {
                //
                labOption.Text = "Table";
                labOption.Location = new Point(0, 0);
                form.panel.Controls.Add(labOption);

                butList[0].AutoSize = false;
                butList[0].Width = 50;
                butList[0].Location = new Point(0, 25);
                butList[0].Text = "MTT";
                form.panel.Controls.Add(butList[0]);

                butList[1].AutoSize = false;
                butList[1].Width = 50;
                butList[1].Location = new Point(50, 25);
                butList[1].Text = "Cash";
                form.panel.Controls.Add(butList[1]);
            }

            void LoadButton(Table _table)
            {
                labOption.Text = "Hero";
                labOption.Location = new Point(0, 0);
                form.panel.Controls.Add(labOption);

                //Remaining are the same

                butList[0].AutoSize = false;
                butList[0].Width = 35;
                butList[0].Location = new Point(0, 25);
                butList[0].Text = "BB";
                form.panel.Controls.Add(butList[0]);

                butList[1].AutoSize = false;
                butList[1].Width = 35;
                butList[1].Location = new Point(35, 25);
                butList[1].Text = "SB";
                form.panel.Controls.Add(butList[1]);

                butList[2].AutoSize = false;
                butList[2].Width = 35;
                butList[2].Location = new Point(70, 25);
                butList[2].Text = "BN";
                form.panel.Controls.Add(butList[2]);

                butList[3].AutoSize = false;
                butList[3].Width = 35;
                butList[3].Location = new Point(105, 25);
                butList[3].Text = "CO";
                form.panel.Controls.Add(butList[3]);

                butList[4].AutoSize = false;
                butList[4].Width = 35;
                butList[4].Location = new Point(140, 25);
                butList[4].Text = "HJ";
                form.panel.Controls.Add(butList[4]);

                butList[5].AutoSize = false;
                butList[5].Width = 35;
                butList[5].Location = new Point(175, 25);
                butList[5].Text = "LJ";
                form.panel.Controls.Add(butList[5]);

                if(table == Table.MTT)
                { 
                    butList[6].AutoSize = false;
                    butList[6].Width = 35;
                    butList[6].Location = new Point(210, 25);
                    butList[6].Text = "U2";
                    form.panel.Controls.Add(butList[6]);

                    butList[7].AutoSize = false;
                    butList[7].Width = 35;
                    butList[7].Location = new Point(245, 25);
                    butList[7].Text = "U1";
                    form.panel.Controls.Add(butList[7]);

                    butList[8].AutoSize = false;
                    butList[8].Width = 35;
                    butList[8].Location = new Point(280, 25);
                    butList[8].Text = "U";
                    form.panel.Controls.Add(butList[8]);
                }

                //Selected
                LoadSelected(table, posHero, action, posVillain, chip);
            }

            void LoadButton(Table _table, Position _hero)
            {
                labOption.Text = "Action";
                labOption.Location = new Point(0, 0);
                form.panel.Controls.Add(labOption);
                //BB no rfi && v3b
                //6-LJ no def && no v4
                //9-UTG no def && no v4

                //basic FI def v3 v4 
                if (_hero != Position.BB)
                {
                    butList[0].AutoSize = false;
                    butList[0].Width = 40;
                    butList[0].Location = new Point(0, 25);
                    butList[0].Text = "FI";
                    form.panel.Controls.Add(butList[0]);

                    butList[2].AutoSize = false;
                    butList[2].Width = 40;
                    butList[2].Location = new Point(80, 25);
                    butList[2].Text = "v3bet";
                    form.panel.Controls.Add(butList[2]);
                }
                else
                {
                    if (_table == Table.MTT)
                    {
                        butList[7].AutoSize = false;
                        butList[7].Width = 40;
                        butList[7].Location = new Point(280, 25);
                        butList[7].Text = "VLP";
                        form.panel.Controls.Add(butList[7]);
                    }
                    butList[5].AutoSize = false;
                    butList[5].Width = 40;
                    butList[5].Location = new Point(200, 25);
                    butList[5].Text = "VL";
                    form.panel.Controls.Add(butList[5]);

                    butList[6].AutoSize = false;
                    butList[6].Width = 40;
                    butList[6].Location = new Point(240, 25);
                    butList[6].Text = "VLR";
                    form.panel.Controls.Add(butList[6]);
                }

                if (!(_hero == Position.LJ && _table == Table.Cash) && !(_hero == Position.UTG && _table == Table.MTT))
                {
                    butList[1].AutoSize = false;
                    butList[1].Width = 40;
                    butList[1].Location = new Point(40, 25);
                    butList[1].Text = "Def";
                    form.panel.Controls.Add(butList[1]);

                    butList[3].AutoSize = false;
                    butList[3].Width = 40;
                    butList[3].Location = new Point(120, 25);
                    butList[3].Text = "v4bet";
                    form.panel.Controls.Add(butList[3]);
                }

                //RVP MTT only
                if(_table == Table.MTT)
                {
                    if (_hero != Position.BB)
                    {
                        //No SB 15
                        butList[4].AutoSize = false;
                        butList[4].Width = 40;
                        butList[4].Location = new Point(160, 25);
                        butList[4].Text = "RVP";
                        form.panel.Controls.Add(butList[4]);
                    }
                    else
                    {
                        butList[4].AutoSize = false;
                        butList[4].Width = 40;
                        butList[4].Location = new Point(160, 25);
                        butList[4].Text = "VOP";
                        form.panel.Controls.Add(butList[4]);
                    }
                }

                if(_hero == Position.SB)
                {
                    //Limp vs Push (MTT)
                    if(_table == Table.MTT)
                    {
                        butList[6].AutoSize = false;
                        butList[6].Width = 40;
                        butList[6].Location = new Point(240, 25);
                        butList[6].Text = "LVP";
                        form.panel.Controls.Add(butList[6]);
                    }
                    butList[5].AutoSize = false;
                    butList[5].Width = 40;
                    butList[5].Location = new Point(200, 25);
                    butList[5].Text = "LVR";
                    form.panel.Controls.Add(butList[5]);
                }

                LoadSelected(table, posHero, action, posVillain, chip);
            }


            void LoadButton(Table _table, Position _hero, Action _action)
            {
                labOption.Text = "Villain";
                labOption.Location = new Point(0, 0);
                form.panel.Controls.Add(labOption);
                //BB no rfi && v3b
                //6-LJ no def && no v4
                //9-UTG no def && no v4
                switch (_action)
                {
                    case Action.FI:
                        //End Cash FI冇後
                        if (table == Table.Cash)
                        {
                            filePic = GetFileName();

                            string path = AppDomain.CurrentDomain.BaseDirectory
                            + @"\PreflopTables\" + filePic + ".png";

                            //form.TabPreflop.Text = filePic;

                            if (File.Exists(path))
                            {
                                Image tempImg = Image.FromFile(path);

                                pic.Image = tempImg;

                                pic.Width = 320;
                                pic.Height = (int)((double)tempImg.Height / (double)tempImg.Width * (double)pic.Width);
                            }

                            form.RandomPercent();
                            break;
                        }
                        else
                        {
                            butList[0].AutoSize = false;
                            butList[0].Width = 35;
                            butList[0].Location = new Point(0, 25);
                            butList[0].Text = "15";
                            form.panel.Controls.Add(butList[0]);

                            butList[1].AutoSize = false;
                            butList[1].Width = 35;
                            butList[1].Location = new Point(35, 25);
                            butList[1].Text = "25";
                            form.panel.Controls.Add(butList[1]);

                            butList[2].AutoSize = false;
                            butList[2].Width = 35;
                            butList[2].Location = new Point(70, 25);
                            butList[2].Text = "40";
                            form.panel.Controls.Add(butList[2]);

                            butList[3].AutoSize = false;
                            butList[3].Width = 35;
                            butList[3].Location = new Point(105, 25);
                            butList[3].Text = "60";
                            form.panel.Controls.Add(butList[3]);
                        }
                        break;
                    case Action.LVP:
                    case Action.LVR:
                        butList[0].AutoSize = false;
                        butList[0].Width = 35;
                        butList[0].Location = new Point(0, 25);
                        butList[0].Text = "BB";
                        form.panel.Controls.Add(butList[0]);
                        break;
                    case Action.VLP:
                    case Action.VLR:
                    case Action.VOP:
                        butList[1].AutoSize = false;
                        butList[1].Width = 35;
                        butList[1].Location = new Point(35, 25);
                        butList[1].Text = "SB";
                        form.panel.Controls.Add(butList[1]);
                        break;
                    case Action.RVP:
                    case Action.V3B:
                        if ((int)_hero > (int)Position.BB)
                        {
                            butList[0].AutoSize = false;
                            butList[0].Width = 35;
                            butList[0].Location = new Point(0, 25);
                            butList[0].Text = "BB";
                            form.panel.Controls.Add(butList[0]);
                        }
                        if ((int)_hero > (int)Position.SB)
                        {
                            butList[1].AutoSize = false;
                            butList[1].Width = 35;
                            butList[1].Location = new Point(35, 25);
                            butList[1].Text = "SB";
                            form.panel.Controls.Add(butList[1]);
                        }
                        if ((int)_hero > (int)Position.BN)
                        {
                            butList[2].AutoSize = false;
                            butList[2].Width = 35;
                            butList[2].Location = new Point(70, 25);
                            butList[2].Text = "BN";
                            form.panel.Controls.Add(butList[2]);
                        }
                        if ((int)_hero > (int)Position.CO)
                        {
                            butList[3].AutoSize = false;
                            butList[3].Width = 35;
                            butList[3].Location = new Point(105, 25);
                            butList[3].Text = "CO";
                            form.panel.Controls.Add(butList[3]);
                        }
                        if ((int)_hero > (int)Position.HJ)
                        {
                            butList[4].AutoSize = false;
                            butList[4].Width = 35;
                            butList[4].Location = new Point(140, 25);
                            butList[4].Text = "HJ";
                            form.panel.Controls.Add(butList[4]);
                        }
                        if (table == Table.MTT)
                        {
                            if ((int)_hero > (int)Position.LJ)
                            {
                                butList[5].AutoSize = false;
                                butList[5].Width = 35;
                                butList[5].Location = new Point(175, 25);
                                butList[5].Text = "LJ";
                                form.panel.Controls.Add(butList[5]);
                            }

                            if ((int)_hero > (int)Position.UTG2)
                            {
                                butList[6].AutoSize = false;
                                butList[6].Width = 35;
                                butList[6].Location = new Point(210, 25);
                                butList[6].Text = "U2";
                                form.panel.Controls.Add(butList[6]);
                            }

                            if ((int)_hero > (int)Position.UTG1)
                            {
                                butList[7].AutoSize = false;
                                butList[7].Width = 35;
                                butList[7].Location = new Point(245, 25);
                                butList[7].Text = "U1";
                                form.panel.Controls.Add(butList[7]);
                            }
                        }
                        break;
                    case Action.Def:
                    case Action.V4B:
                        if ((int)_hero < (int)Position.SB)
                        {
                            butList[1].AutoSize = false;
                            butList[1].Width = 35;
                            butList[1].Location = new Point(35, 25);
                            butList[1].Text = "SB";
                            form.panel.Controls.Add(butList[1]);
                        }
                        if ((int)_hero < (int)Position.BN)
                        {
                                butList[2].AutoSize = false;
                            butList[2].Width = 35;
                            butList[2].Location = new Point(70, 25);
                            butList[2].Text = "BN";
                            form.panel.Controls.Add(butList[2]);
                        }
                        if ((int)_hero < (int)Position.CO)
                        {
                            butList[3].AutoSize = false;
                            butList[3].Width = 35;
                            butList[3].Location = new Point(105, 25);
                            butList[3].Text = "CO";
                            form.panel.Controls.Add(butList[3]);
                        }
                        if ((int)_hero < (int)Position.HJ)
                        {
                            butList[4].AutoSize = false;
                            butList[4].Width = 35;
                            butList[4].Location = new Point(140, 25);
                            butList[4].Text = "HJ";
                            form.panel.Controls.Add(butList[4]);
                        }
                        if ((int)_hero < (int)Position.LJ)
                        {
                            butList[5].AutoSize = false;
                            butList[5].Width = 35;
                            butList[5].Location = new Point(175, 25);
                            butList[5].Text = "LJ";
                            form.panel.Controls.Add(butList[5]);
                        }
                        
                        if (table == Table.MTT)
                        {
                            if ((int)_hero < (int)Position.UTG2)
                            {
                                butList[6].AutoSize = false;
                                butList[6].Width = 35;
                                butList[6].Location = new Point(210, 25);
                                butList[6].Text = "U2";
                                form.panel.Controls.Add(butList[6]);
                            }

                            if ((int)_hero < (int)Position.UTG1)
                            {
                                butList[7].AutoSize = false;
                                butList[7].Width = 35;
                                butList[7].Location = new Point(245, 25);
                                butList[7].Text = "U1";
                                form.panel.Controls.Add(butList[7]);
                            }

                            if ((int)_hero < (int)Position.UTG)
                            {
                                butList[8].AutoSize = false;
                                butList[8].Width = 35;
                                butList[8].Location = new Point(280, 25);
                                butList[8].Text = "U";
                                form.panel.Controls.Add(butList[8]);
                            }
                        }
                        break;
                }

                LoadSelected(table, posHero, action, posVillain, chip);
            }

            void LoadButton(Table _table, Position _hero, Action _action, int _chip)
            {
                //MTT FI only
                //Load new Image
                filePic = GetFileName();

                string path = AppDomain.CurrentDomain.BaseDirectory
                        + @"\PreflopTables\" + filePic + ".png";

                if (File.Exists(path))
                {
                    Image tempImg = Image.FromFile(path);

                    pic.Image = tempImg;

                    pic.Width = 320;
                    pic.Height = (int)((double)tempImg.Height / (double)tempImg.Width * (double)pic.Width);
                }

                form.RandomPercent();

                LoadSelected(table, posHero, action, posVillain, chip);
            }
            void LoadButton(Table _table, Position _hero, Action _action, Position _villain)
            {
                //Cash = End
                if (table == Table.Cash)
                {
                    //Load new Image
                    filePic = GetFileName();

                    string path = AppDomain.CurrentDomain.BaseDirectory
                            + @"\PreflopTables\" + filePic + ".png";

                    if (File.Exists(path))
                    {
                        Image tempImg = Image.FromFile(path);

                        pic.Image = tempImg;

                        pic.Width = 320;
                        pic.Height = (int)((double)tempImg.Height / (double)tempImg.Width * (double)pic.Width);
                    }

                    form.RandomPercent();
                }
                else
                {
                    if (_action != Action.VLR)
                    {
                        if (!(_hero == Position.SB && _action == Action.RVP))
                        {
                            butList[0].AutoSize = false;
                            butList[0].Width = 35;
                            butList[0].Location = new Point(0, 25);
                            butList[0].Text = "15";
                            form.panel.Controls.Add(butList[0]);
                        }

                        butList[1].AutoSize = false;
                        butList[1].Width = 35;
                        butList[1].Location = new Point(35, 25);
                        butList[1].Text = "25";
                        form.panel.Controls.Add(butList[1]);
                    }

                    if (_action != Action.LVP && _action != Action.RVP && _action != Action.VOP && _action != Action.VLP)
                    {
                        butList[2].AutoSize = false;
                        butList[2].Width = 35;
                        butList[2].Location = new Point(70, 25);
                        butList[2].Text = "40";
                        form.panel.Controls.Add(butList[2]);

                        butList[3].AutoSize = false;
                        butList[3].Width = 35;
                        butList[3].Location = new Point(105, 25);
                        butList[3].Text = "60";
                        form.panel.Controls.Add(butList[3]);
                    }
                }

                LoadSelected(table, posHero, action, posVillain, chip);
            }

            void LoadButton(Table _table, Position _hero, Action _action, Position _villain, int _chip)
            {
                //MTT Def No HJ UTG2-UTG1
                //BB SB BN CO HJ UTG1
                if(table == Table.MTT)
                {
                    if (action == Action.Def)
                    {
                        if (posVillain == Position.HJ)
                            posVillain = Position.LJ;
                        if (posVillain == Position.UTG1 || posVillain == Position.UTG2)
                            posVillain = Position.UTG;

                        if (posHero == Position.UTG2 || posHero == Position.LJ)
                            posHero = Position.UTG1;
                    }
                    else if(action == Action.RVP)
                    {
                        if (posHero == Position.HJ)
                            posHero = Position.LJ;
                        if (posHero == Position.UTG1 || posVillain == Position.UTG2)
                            posHero = Position.UTG;

                        if (posVillain == Position.UTG2 || posHero == Position.LJ)
                            posVillain = Position.UTG1;
                        if (posVillain == Position.CO)
                            posVillain = Position.HJ;
                    }
                }


                //Load new Image
                filePic = GetFileName();

                string path = AppDomain.CurrentDomain.BaseDirectory
                        + @"\PreflopTables\" + filePic + ".png";

                if (File.Exists(path))
                {
                    Image tempImg = Image.FromFile(path);

                    pic.Image = tempImg;

                    pic.Width = 320;
                    pic.Height = (int)((double)tempImg.Height / (double)tempImg.Width * (double)pic.Width);
                }

                form.RandomPercent();

                LoadSelected(table, posHero, action, posVillain, chip);
            }

            void LoadSelected(Table _table, Position _hero, Action _action, Position _villain, int _chip)
            {
                if (_table != Table.none)
                {
                    //Table
                    butSelectedTable.Text = GetString(_table);
                    butSelectedTable.AutoSize = false;
                    butSelectedTable.Width = 50;
                    butSelectedTable.Location = new Point(0, 50);
                    form.panel.Controls.Add(butSelectedTable);

                    if(_hero != Position.none)
                    {
                        //Hero
                        butSelectedHero.Text = GetString(_hero, true);
                        butSelectedHero.AutoSize = false;
                        butSelectedHero.Width = 35;
                        butSelectedHero.Location = new Point(50, 50);
                        form.panel.Controls.Add(butSelectedHero);

                        if (_action != Action.none)
                        {
                            //Hero
                            butSelectedAction.Text = GetString(_action);
                            butSelectedAction.AutoSize = false;
                            butSelectedAction.Width = 50;
                            butSelectedAction.Location = new Point(85, 50);
                            form.panel.Controls.Add(butSelectedAction);

                            if (_villain != Position.none)
                            {
                                //Hero
                                butSelectedFourth.Text = GetString(_villain, true);
                                butSelectedFourth.AutoSize = false;
                                butSelectedFourth.Width = 35;
                                butSelectedFourth.Location = new Point(135, 50);
                                form.panel.Controls.Add(butSelectedFourth);

                                if (_chip != 0)
                                {
                                    //Hero
                                    butSelectedFifth.Text = "" + _chip;
                                    butSelectedFifth.AutoSize = false;
                                    butSelectedFifth.Width = 35;
                                    butSelectedFifth.Location = new Point(170, 50);
                                    form.panel.Controls.Add(butSelectedFifth);

                                }
                            }
                            else if (_chip != 0)
                            {
                                //Hero
                                butSelectedFourth.Text = ""+_chip;
                                butSelectedFourth.AutoSize = false;
                                butSelectedFourth.Width = 35;
                                butSelectedFourth.Location = new Point(135, 50);
                                form.panel.Controls.Add(butSelectedFourth);

                            }
                        }
                    }
                }
            }
            
            string GetFileName()
            {
                string output = GetString(table);

                output += GetString(posHero, false);
                output += GetString(action);
                output += GetString(posVillain, false);

                if(chip != 0)
                {
                    output += chip.ToString();
                }
                //如有Chip TODO

                return output;
            }


            void ButtonOnClick(object sender, EventArgs e)
            {
                //每個Button分別Check
                //var _button = sender as Button;

                if (sender == butSelectedTable)
                {
                    table = Table.none;
                    posHero = Position.none;
                    action = Action.none;
                    posVillain = Position.none;
                    chip = 0;
                }
                if (sender == butSelectedHero)
                {
                    posHero = Position.none;
                    action = Action.none;
                    posVillain = Position.none;
                    chip = 0;
                }
                if (sender == butSelectedAction)
                {
                    action = Action.none;
                    posVillain = Position.none;
                    chip = 0;
                }
                if (sender == butSelectedFourth)
                {
                    posVillain = Position.none;
                    chip = 0;
                }
                if (sender == butSelectedFifth)
                {
                    chip = 0;
                }

                //The rest are array buttons
                if(table != Table.none)
                {
                    if(posHero != Position.none)
                    {
                        if (action != Action.none)
                        {
                            if (posVillain != Position.none)
                            {
                                //!FI
                                if (chip == 0)
                                {
                                    // Chip
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (sender == butList[i])
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    chip = 15;
                                                    break;
                                                case 1:
                                                    chip = 25;
                                                    break;
                                                case 2:
                                                    chip = 40;
                                                    break;
                                                case 3:
                                                    chip = 60;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //FI
                                if (table == Table.MTT && action == Action.FI && chip == 0)
                                {
                                    // Chip
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (sender == butList[i])
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    chip = 15;
                                                    break;
                                                case 1:
                                                    chip = 25;
                                                    break;
                                                case 2:
                                                    chip = 40;
                                                    break;
                                                case 3:
                                                    chip = 60;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                }

                                else
                                {
                                    //Hero
                                    for (int i = 0; i < 9; i++)
                                    {
                                        if (sender == butList[i])
                                        {
                                            posVillain = (Position)(i + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Action
                            for (int i = 0; i < 8; i++)
                            {
                                if (sender == butList[i])
                                {
                                    if (posHero == Position.BB && (i) >= (int)Action.V4B)
                                    {
                                        action = (Action)((i) -(int)Action.V4B + (int)Action.VOP);
                                    }
                                    else
                                    {
                                        action = (Action)(i + 1);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Hero
                        for (int i = 0; i < 9; i++)
                        {
                            if (sender == butList[i])
                            {
                                posHero = (Position)(i + 1);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for(int i = 0;i < 2;i++)//MTT and Cash
                    {
                        if(sender == butList[i])
                        {
                            table = (Table)(i + 1);
                            break;
                        }
                    }
                }

                Load();
            }
        }

        public class Flop : TabPage
        {

        }
        public class Turn : TabPage
        {

        }

        public static string GetString(Preflop.Action _action)
        {
            switch (_action)
            {
                case Preflop.Action.FI:
                    return "FI";
                case Preflop.Action.Def:
                    return "Def";
                case Preflop.Action.V3B:
                    return "V3B";
                case Preflop.Action.V4B:
                    return "V4B";
                case Preflop.Action.RVP:
                    return "RVP";
                case Preflop.Action.LVP:
                    return "LVP";
                case Preflop.Action.LVR:
                    return "LVR";
                case Preflop.Action.VOP:
                    return "VOP";
                case Preflop.Action.VL:
                    return "VL";
                case Preflop.Action.VLR:
                    return "VLR";
                case Preflop.Action.VLP:
                    return "VLP";
            }
            return "";
        }

        public static string GetString(Preflop.Table _table)
        {
            switch(_table)
            {
                case Preflop.Table.Cash:
                    return "Cash";
                case Preflop.Table.MTT:
                    return "MTT";
            }
            return "";
        }
        public static string GetString(Preflop.Position _pos, bool isShort)
        {
            switch(_pos)
            {
                case Preflop.Position.none:
                    return "";
                case Preflop.Position.BB:
                    return "BB";
                case Preflop.Position.SB:
                    return "SB";
                case Preflop.Position.BN:
                    return "BN";
                case Preflop.Position.CO:
                    return "CO";
                case Preflop.Position.HJ:
                    return "HJ";
                case Preflop.Position.LJ:
                    return "LJ";
                case Preflop.Position.UTG2:
                    if (isShort)
                        return "U2";
                    return "UTG2";
                case Preflop.Position.UTG1:
                    if (isShort)
                        return "U1";
                    return "UTG1";
                case Preflop.Position.UTG:
                    if (isShort)
                        return "U";
                    return "UTG";
            }
            return null;
        }
        #endregion

        public void RandomPercent()
        {
            panRand.Random();
        }
    }
}

