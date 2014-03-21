namespace Analyzer.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this._chbCheck = new System.Windows.Forms.CheckBox();
            this._lbResult = new System.Windows.Forms.ListBox();
            this._gbGrammarType = new System.Windows.Forms.GroupBox();
            this._rbContexFree = new System.Windows.Forms.RadioButton();
            this._rbRegular = new System.Windows.Forms.RadioButton();
            this._btBuildGrammar = new System.Windows.Forms.Button();
            this._btDeleteRule = new System.Windows.Forms.Button();
            this._lbExistingRulesView = new System.Windows.Forms.ListBox();
            this._lbExistingRules = new System.Windows.Forms.Label();
            this._btAddRule = new System.Windows.Forms.Button();
            this._lbInputRule = new System.Windows.Forms.Label();
            this._tbNewRule = new System.Windows.Forms.TextBox();
            this._lbPickNonTerminal = new System.Windows.Forms.Label();
            this._cbNonTerminals = new System.Windows.Forms.ComboBox();
            this._btAddNonTerminal = new System.Windows.Forms.Button();
            this._lbNonTerminalName = new System.Windows.Forms.Label();
            this._tbNonTerminalName = new System.Windows.Forms.TextBox();
            this._tbText = new System.Windows.Forms.TextBox();
            this._tcTabs = new System.Windows.Forms.TabControl();
            this._tpCreation = new System.Windows.Forms.TabPage();
            this._lbBuildTime = new System.Windows.Forms.Label();
            this._tbTimer = new System.Windows.Forms.TextBox();
            this._tbTransitionsCount = new System.Windows.Forms.TextBox();
            this._lbTransitionsCount = new System.Windows.Forms.Label();
            this._lbStatesCount = new System.Windows.Forms.Label();
            this._tbStatesCount = new System.Windows.Forms.TextBox();
            this._lbFilename = new System.Windows.Forms.Label();
            this._tbFilename = new System.Windows.Forms.TextBox();
            this._btLoad = new System.Windows.Forms.Button();
            this._btSave = new System.Windows.Forms.Button();
            this._btAgregate = new System.Windows.Forms.Button();
            this._gbAutomatonType = new System.Windows.Forms.GroupBox();
            this._rbDFA = new System.Windows.Forms.RadioButton();
            this._rbDFAconverted = new System.Windows.Forms.RadioButton();
            this._rbNFA = new System.Windows.Forms.RadioButton();
            this._btReset = new System.Windows.Forms.Button();
            this._btAddCapitalLetters = new System.Windows.Forms.Button();
            this._btAddDigits = new System.Windows.Forms.Button();
            this._btAddLetters = new System.Windows.Forms.Button();
            this._tpUsage = new System.Windows.Forms.TabPage();
            this._tbWorktime = new System.Windows.Forms.TextBox();
            this._lbWorktime = new System.Windows.Forms.Label();
            this._lbInputText = new System.Windows.Forms.Label();
            this._btCheckText = new System.Windows.Forms.Button();
            this._tpHelp = new System.Windows.Forms.TabPage();
            this._gbGrammarType.SuspendLayout();
            this._tcTabs.SuspendLayout();
            this._tpCreation.SuspendLayout();
            this._gbAutomatonType.SuspendLayout();
            this._tpUsage.SuspendLayout();
            this._tpHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(632, 376);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // _chbCheck
            // 
            this._chbCheck.AutoSize = true;
            this._chbCheck.Location = new System.Drawing.Point(488, 70);
            this._chbCheck.Name = "_chbCheck";
            this._chbCheck.Size = new System.Drawing.Size(98, 17);
            this._chbCheck.TabIndex = 19;
            this._chbCheck.Text = "Распознавать";
            this._chbCheck.UseVisualStyleBackColor = true;
            // 
            // _lbResult
            // 
            this._lbResult.FormattingEnabled = true;
            this._lbResult.Location = new System.Drawing.Point(9, 272);
            this._lbResult.Name = "_lbResult";
            this._lbResult.Size = new System.Drawing.Size(637, 108);
            this._lbResult.TabIndex = 18;
            // 
            // _gbGrammarType
            // 
            this._gbGrammarType.Controls.Add(this._rbContexFree);
            this._gbGrammarType.Controls.Add(this._rbRegular);
            this._gbGrammarType.Location = new System.Drawing.Point(6, 8);
            this._gbGrammarType.Name = "_gbGrammarType";
            this._gbGrammarType.Size = new System.Drawing.Size(151, 91);
            this._gbGrammarType.TabIndex = 13;
            this._gbGrammarType.TabStop = false;
            this._gbGrammarType.Text = "Тип грамматики";
            // 
            // _rbContexFree
            // 
            this._rbContexFree.AutoSize = true;
            this._rbContexFree.Location = new System.Drawing.Point(6, 55);
            this._rbContexFree.Name = "_rbContexFree";
            this._rbContexFree.Size = new System.Drawing.Size(141, 17);
            this._rbContexFree.TabIndex = 1;
            this._rbContexFree.TabStop = true;
            this._rbContexFree.Text = "Контекстно-свободная";
            this._rbContexFree.UseVisualStyleBackColor = true;
            // 
            // _rbRegular
            // 
            this._rbRegular.AutoSize = true;
            this._rbRegular.Location = new System.Drawing.Point(6, 32);
            this._rbRegular.Name = "_rbRegular";
            this._rbRegular.Size = new System.Drawing.Size(84, 17);
            this._rbRegular.TabIndex = 0;
            this._rbRegular.TabStop = true;
            this._rbRegular.Text = "Регулярная";
            this._rbRegular.UseVisualStyleBackColor = true;
            // 
            // _btBuildGrammar
            // 
            this._btBuildGrammar.Location = new System.Drawing.Point(490, 276);
            this._btBuildGrammar.Name = "_btBuildGrammar";
            this._btBuildGrammar.Size = new System.Drawing.Size(150, 25);
            this._btBuildGrammar.TabIndex = 12;
            this._btBuildGrammar.Text = "Построить грамматику";
            this._btBuildGrammar.UseVisualStyleBackColor = true;
            // 
            // _btDeleteRule
            // 
            this._btDeleteRule.Location = new System.Drawing.Point(490, 150);
            this._btDeleteRule.Name = "_btDeleteRule";
            this._btDeleteRule.Size = new System.Drawing.Size(150, 25);
            this._btDeleteRule.TabIndex = 11;
            this._btDeleteRule.Text = "Удалить правило";
            this._btDeleteRule.UseVisualStyleBackColor = true;
            // 
            // _lbExistingRulesView
            // 
            this._lbExistingRulesView.FormattingEnabled = true;
            this._lbExistingRulesView.HorizontalScrollbar = true;
            this._lbExistingRulesView.Location = new System.Drawing.Point(177, 150);
            this._lbExistingRulesView.Name = "_lbExistingRulesView";
            this._lbExistingRulesView.Size = new System.Drawing.Size(305, 95);
            this._lbExistingRulesView.TabIndex = 10;
            // 
            // _lbExistingRules
            // 
            this._lbExistingRules.AutoSize = true;
            this._lbExistingRules.Location = new System.Drawing.Point(174, 134);
            this._lbExistingRules.Name = "_lbExistingRules";
            this._lbExistingRules.Size = new System.Drawing.Size(130, 13);
            this._lbExistingRules.TabIndex = 8;
            this._lbExistingRules.Text = "Существующие правила";
            // 
            // _btAddRule
            // 
            this._btAddRule.Location = new System.Drawing.Point(490, 98);
            this._btAddRule.Name = "_btAddRule";
            this._btAddRule.Size = new System.Drawing.Size(150, 25);
            this._btAddRule.TabIndex = 7;
            this._btAddRule.Text = "Добавить правило";
            this._btAddRule.UseVisualStyleBackColor = true;
            // 
            // _lbInputRule
            // 
            this._lbInputRule.AutoSize = true;
            this._lbInputRule.Location = new System.Drawing.Point(171, 75);
            this._lbInputRule.Name = "_lbInputRule";
            this._lbInputRule.Size = new System.Drawing.Size(94, 13);
            this._lbInputRule.TabIndex = 6;
            this._lbInputRule.Text = "Введите правило";
            // 
            // _tbNewRule
            // 
            this._tbNewRule.Location = new System.Drawing.Point(174, 100);
            this._tbNewRule.Name = "_tbNewRule";
            this._tbNewRule.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this._tbNewRule.Size = new System.Drawing.Size(308, 20);
            this._tbNewRule.TabIndex = 5;
            // 
            // _lbPickNonTerminal
            // 
            this._lbPickNonTerminal.AutoSize = true;
            this._lbPickNonTerminal.Location = new System.Drawing.Point(485, 8);
            this._lbPickNonTerminal.Name = "_lbPickNonTerminal";
            this._lbPickNonTerminal.Size = new System.Drawing.Size(121, 13);
            this._lbPickNonTerminal.TabIndex = 4;
            this._lbPickNonTerminal.Text = "Выберете нетерминал";
            // 
            // _cbNonTerminals
            // 
            this._cbNonTerminals.FormattingEnabled = true;
            this._cbNonTerminals.Location = new System.Drawing.Point(488, 43);
            this._cbNonTerminals.Name = "_cbNonTerminals";
            this._cbNonTerminals.Size = new System.Drawing.Size(152, 21);
            this._cbNonTerminals.TabIndex = 3;
            // 
            // _btAddNonTerminal
            // 
            this._btAddNonTerminal.Location = new System.Drawing.Point(332, 40);
            this._btAddNonTerminal.Name = "_btAddNonTerminal";
            this._btAddNonTerminal.Size = new System.Drawing.Size(150, 25);
            this._btAddNonTerminal.TabIndex = 2;
            this._btAddNonTerminal.Text = "Добавить нетерминал";
            this._btAddNonTerminal.UseVisualStyleBackColor = true;
            // 
            // _lbNonTerminalName
            // 
            this._lbNonTerminalName.AutoSize = true;
            this._lbNonTerminalName.Location = new System.Drawing.Point(171, 26);
            this._lbNonTerminalName.Name = "_lbNonTerminalName";
            this._lbNonTerminalName.Size = new System.Drawing.Size(99, 13);
            this._lbNonTerminalName.TabIndex = 1;
            this._lbNonTerminalName.Text = "Имя нетерминала";
            this._lbNonTerminalName.Click += new System.EventHandler(this._lbNonTerminalName_Click);
            // 
            // _tbNonTerminalName
            // 
            this._tbNonTerminalName.Location = new System.Drawing.Point(174, 42);
            this._tbNonTerminalName.Name = "_tbNonTerminalName";
            this._tbNonTerminalName.Size = new System.Drawing.Size(152, 20);
            this._tbNonTerminalName.TabIndex = 0;
            // 
            // _tbText
            // 
            this._tbText.Location = new System.Drawing.Point(9, 19);
            this._tbText.Multiline = true;
            this._tbText.Name = "_tbText";
            this._tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbText.Size = new System.Drawing.Size(625, 218);
            this._tbText.TabIndex = 21;
            // 
            // _tcTabs
            // 
            this._tcTabs.Controls.Add(this._tpCreation);
            this._tcTabs.Controls.Add(this._tpUsage);
            this._tcTabs.Controls.Add(this._tpHelp);
            this._tcTabs.Location = new System.Drawing.Point(12, 12);
            this._tcTabs.Name = "_tcTabs";
            this._tcTabs.SelectedIndex = 0;
            this._tcTabs.Size = new System.Drawing.Size(665, 424);
            this._tcTabs.TabIndex = 1;
            // 
            // _tpCreation
            // 
            this._tpCreation.Controls.Add(this._lbBuildTime);
            this._tpCreation.Controls.Add(this._tbTimer);
            this._tpCreation.Controls.Add(this._tbTransitionsCount);
            this._tpCreation.Controls.Add(this._lbTransitionsCount);
            this._tpCreation.Controls.Add(this._lbStatesCount);
            this._tpCreation.Controls.Add(this._tbStatesCount);
            this._tpCreation.Controls.Add(this._lbFilename);
            this._tpCreation.Controls.Add(this._tbFilename);
            this._tpCreation.Controls.Add(this._btLoad);
            this._tpCreation.Controls.Add(this._btSave);
            this._tpCreation.Controls.Add(this._btAgregate);
            this._tpCreation.Controls.Add(this._gbAutomatonType);
            this._tpCreation.Controls.Add(this._btReset);
            this._tpCreation.Controls.Add(this._btAddCapitalLetters);
            this._tpCreation.Controls.Add(this._btAddDigits);
            this._tpCreation.Controls.Add(this._btAddLetters);
            this._tpCreation.Controls.Add(this._btBuildGrammar);
            this._tpCreation.Controls.Add(this._gbGrammarType);
            this._tpCreation.Controls.Add(this._chbCheck);
            this._tpCreation.Controls.Add(this._tbNonTerminalName);
            this._tpCreation.Controls.Add(this._lbNonTerminalName);
            this._tpCreation.Controls.Add(this._btAddNonTerminal);
            this._tpCreation.Controls.Add(this._btDeleteRule);
            this._tpCreation.Controls.Add(this._cbNonTerminals);
            this._tpCreation.Controls.Add(this._lbExistingRulesView);
            this._tpCreation.Controls.Add(this._lbPickNonTerminal);
            this._tpCreation.Controls.Add(this._lbExistingRules);
            this._tpCreation.Controls.Add(this._tbNewRule);
            this._tpCreation.Controls.Add(this._btAddRule);
            this._tpCreation.Controls.Add(this._lbInputRule);
            this._tpCreation.Location = new System.Drawing.Point(4, 22);
            this._tpCreation.Name = "_tpCreation";
            this._tpCreation.Padding = new System.Windows.Forms.Padding(3);
            this._tpCreation.Size = new System.Drawing.Size(657, 398);
            this._tpCreation.TabIndex = 0;
            this._tpCreation.Text = "Создание";
            this._tpCreation.UseVisualStyleBackColor = true;
            // 
            // _lbBuildTime
            // 
            this._lbBuildTime.AutoSize = true;
            this._lbBuildTime.Location = new System.Drawing.Point(174, 363);
            this._lbBuildTime.Name = "_lbBuildTime";
            this._lbBuildTime.Size = new System.Drawing.Size(139, 13);
            this._lbBuildTime.TabIndex = 43;
            this._lbBuildTime.Text = "Общее время построения";
            // 
            // _tbTimer
            // 
            this._tbTimer.Location = new System.Drawing.Point(324, 360);
            this._tbTimer.Name = "_tbTimer";
            this._tbTimer.ReadOnly = true;
            this._tbTimer.Size = new System.Drawing.Size(100, 20);
            this._tbTimer.TabIndex = 42;
            // 
            // _tbTransitionsCount
            // 
            this._tbTransitionsCount.Location = new System.Drawing.Point(324, 326);
            this._tbTransitionsCount.Name = "_tbTransitionsCount";
            this._tbTransitionsCount.ReadOnly = true;
            this._tbTransitionsCount.Size = new System.Drawing.Size(100, 20);
            this._tbTransitionsCount.TabIndex = 41;
            // 
            // _lbTransitionsCount
            // 
            this._lbTransitionsCount.AutoSize = true;
            this._lbTransitionsCount.Location = new System.Drawing.Point(174, 326);
            this._lbTransitionsCount.Name = "_lbTransitionsCount";
            this._lbTransitionsCount.Size = new System.Drawing.Size(130, 13);
            this._lbTransitionsCount.TabIndex = 40;
            this._lbTransitionsCount.Text = "Общее число переходов";
            // 
            // _lbStatesCount
            // 
            this._lbStatesCount.AutoSize = true;
            this._lbStatesCount.Location = new System.Drawing.Point(174, 291);
            this._lbStatesCount.Name = "_lbStatesCount";
            this._lbStatesCount.Size = new System.Drawing.Size(130, 13);
            this._lbStatesCount.TabIndex = 39;
            this._lbStatesCount.Text = "Общее число состояний";
            // 
            // _tbStatesCount
            // 
            this._tbStatesCount.Location = new System.Drawing.Point(324, 288);
            this._tbStatesCount.Name = "_tbStatesCount";
            this._tbStatesCount.ReadOnly = true;
            this._tbStatesCount.Size = new System.Drawing.Size(100, 20);
            this._tbStatesCount.TabIndex = 38;
            // 
            // _lbFilename
            // 
            this._lbFilename.AutoSize = true;
            this._lbFilename.Location = new System.Drawing.Point(6, 284);
            this._lbFilename.Name = "_lbFilename";
            this._lbFilename.Size = new System.Drawing.Size(64, 13);
            this._lbFilename.TabIndex = 36;
            this._lbFilename.Text = "Имя файла";
            // 
            // _tbFilename
            // 
            this._tbFilename.Location = new System.Drawing.Point(6, 300);
            this._tbFilename.Name = "_tbFilename";
            this._tbFilename.Size = new System.Drawing.Size(150, 20);
            this._tbFilename.TabIndex = 35;
            // 
            // _btLoad
            // 
            this._btLoad.Location = new System.Drawing.Point(6, 357);
            this._btLoad.Name = "_btLoad";
            this._btLoad.Size = new System.Drawing.Size(150, 25);
            this._btLoad.TabIndex = 34;
            this._btLoad.Text = "Загрузить";
            this._btLoad.UseVisualStyleBackColor = true;
            // 
            // _btSave
            // 
            this._btSave.Location = new System.Drawing.Point(6, 326);
            this._btSave.Name = "_btSave";
            this._btSave.Size = new System.Drawing.Size(150, 25);
            this._btSave.TabIndex = 33;
            this._btSave.Text = "Сохранить";
            this._btSave.UseVisualStyleBackColor = true;
            // 
            // _btAgregate
            // 
            this._btAgregate.Location = new System.Drawing.Point(177, 247);
            this._btAgregate.Name = "_btAgregate";
            this._btAgregate.Size = new System.Drawing.Size(165, 25);
            this._btAgregate.TabIndex = 32;
            this._btAgregate.Text = "Собрать все правила в одно";
            this._btAgregate.UseVisualStyleBackColor = true;
            // 
            // _gbAutomatonType
            // 
            this._gbAutomatonType.Controls.Add(this._rbDFA);
            this._gbAutomatonType.Controls.Add(this._rbDFAconverted);
            this._gbAutomatonType.Controls.Add(this._rbNFA);
            this._gbAutomatonType.Location = new System.Drawing.Point(490, 179);
            this._gbAutomatonType.Name = "_gbAutomatonType";
            this._gbAutomatonType.Size = new System.Drawing.Size(150, 91);
            this._gbAutomatonType.TabIndex = 30;
            this._gbAutomatonType.TabStop = false;
            this._gbAutomatonType.Text = "Тип автомата";
            // 
            // _rbDFA
            // 
            this._rbDFA.AutoSize = true;
            this._rbDFA.Location = new System.Drawing.Point(6, 65);
            this._rbDFA.Name = "_rbDFA";
            this._rbDFA.Size = new System.Drawing.Size(133, 17);
            this._rbDFA.TabIndex = 2;
            this._rbDFA.TabStop = true;
            this._rbDFA.Text = "Детерминированный";
            this._rbDFA.UseVisualStyleBackColor = true;
            // 
            // _rbDFAconverted
            // 
            this._rbDFAconverted.AutoSize = true;
            this._rbDFAconverted.Location = new System.Drawing.Point(6, 42);
            this._rbDFAconverted.Name = "_rbDFAconverted";
            this._rbDFAconverted.Size = new System.Drawing.Size(79, 17);
            this._rbDFAconverted.TabIndex = 1;
            this._rbDFAconverted.TabStop = true;
            this._rbDFAconverted.Text = "НКА->ДКА";
            this._rbDFAconverted.UseVisualStyleBackColor = true;
            // 
            // _rbNFA
            // 
            this._rbNFA.AutoSize = true;
            this._rbNFA.Location = new System.Drawing.Point(6, 19);
            this._rbNFA.Name = "_rbNFA";
            this._rbNFA.Size = new System.Drawing.Size(144, 17);
            this._rbNFA.TabIndex = 0;
            this._rbNFA.TabStop = true;
            this._rbNFA.Text = "Недетерминированный";
            this._rbNFA.UseVisualStyleBackColor = true;
            // 
            // _btReset
            // 
            this._btReset.Location = new System.Drawing.Point(6, 247);
            this._btReset.Name = "_btReset";
            this._btReset.Size = new System.Drawing.Size(150, 25);
            this._btReset.TabIndex = 29;
            this._btReset.Text = "Сброс";
            this._btReset.UseVisualStyleBackColor = true;
            // 
            // _btAddCapitalLetters
            // 
            this._btAddCapitalLetters.Location = new System.Drawing.Point(6, 201);
            this._btAddCapitalLetters.Name = "_btAddCapitalLetters";
            this._btAddCapitalLetters.Size = new System.Drawing.Size(150, 25);
            this._btAddCapitalLetters.TabIndex = 22;
            this._btAddCapitalLetters.Text = "Большие буквы";
            this._btAddCapitalLetters.UseVisualStyleBackColor = true;
            // 
            // _btAddDigits
            // 
            this._btAddDigits.Location = new System.Drawing.Point(6, 161);
            this._btAddDigits.Name = "_btAddDigits";
            this._btAddDigits.Size = new System.Drawing.Size(150, 25);
            this._btAddDigits.TabIndex = 21;
            this._btAddDigits.Text = "Цифры";
            this._btAddDigits.UseVisualStyleBackColor = true;
            // 
            // _btAddLetters
            // 
            this._btAddLetters.Location = new System.Drawing.Point(6, 121);
            this._btAddLetters.Name = "_btAddLetters";
            this._btAddLetters.Size = new System.Drawing.Size(150, 25);
            this._btAddLetters.TabIndex = 20;
            this._btAddLetters.Text = "Маленькие буквы";
            this._btAddLetters.UseVisualStyleBackColor = true;
            // 
            // _tpUsage
            // 
            this._tpUsage.Controls.Add(this._tbWorktime);
            this._tpUsage.Controls.Add(this._lbWorktime);
            this._tpUsage.Controls.Add(this._lbInputText);
            this._tpUsage.Controls.Add(this._btCheckText);
            this._tpUsage.Controls.Add(this._tbText);
            this._tpUsage.Controls.Add(this._lbResult);
            this._tpUsage.Location = new System.Drawing.Point(4, 22);
            this._tpUsage.Name = "_tpUsage";
            this._tpUsage.Padding = new System.Windows.Forms.Padding(3);
            this._tpUsage.Size = new System.Drawing.Size(657, 398);
            this._tpUsage.TabIndex = 1;
            this._tpUsage.Text = "Использование";
            this._tpUsage.UseVisualStyleBackColor = true;
            // 
            // _tbWorktime
            // 
            this._tbWorktime.Location = new System.Drawing.Point(184, 246);
            this._tbWorktime.Name = "_tbWorktime";
            this._tbWorktime.Size = new System.Drawing.Size(121, 20);
            this._tbWorktime.TabIndex = 27;
            // 
            // _lbWorktime
            // 
            this._lbWorktime.AutoSize = true;
            this._lbWorktime.Location = new System.Drawing.Point(98, 248);
            this._lbWorktime.Name = "_lbWorktime";
            this._lbWorktime.Size = new System.Drawing.Size(80, 13);
            this._lbWorktime.TabIndex = 26;
            this._lbWorktime.Text = "Время работы";
            // 
            // _lbInputText
            // 
            this._lbInputText.AutoSize = true;
            this._lbInputText.Location = new System.Drawing.Point(6, 3);
            this._lbInputText.Name = "_lbInputText";
            this._lbInputText.Size = new System.Drawing.Size(80, 13);
            this._lbInputText.TabIndex = 25;
            this._lbInputText.Text = "Введите текст";
            // 
            // _btCheckText
            // 
            this._btCheckText.Location = new System.Drawing.Point(9, 243);
            this._btCheckText.Name = "_btCheckText";
            this._btCheckText.Size = new System.Drawing.Size(75, 23);
            this._btCheckText.TabIndex = 24;
            this._btCheckText.Text = "Обработать";
            this._btCheckText.UseVisualStyleBackColor = true;
            // 
            // _tpHelp
            // 
            this._tpHelp.Controls.Add(this.textBox1);
            this._tpHelp.Location = new System.Drawing.Point(4, 22);
            this._tpHelp.Name = "_tpHelp";
            this._tpHelp.Size = new System.Drawing.Size(657, 398);
            this._tpHelp.TabIndex = 2;
            this._tpHelp.Text = "Справка";
            this._tpHelp.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 440);
            this.Controls.Add(this._tcTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.Text = "Лексический анализатор";
            this._gbGrammarType.ResumeLayout(false);
            this._gbGrammarType.PerformLayout();
            this._tcTabs.ResumeLayout(false);
            this._tpCreation.ResumeLayout(false);
            this._tpCreation.PerformLayout();
            this._gbAutomatonType.ResumeLayout(false);
            this._gbAutomatonType.PerformLayout();
            this._tpUsage.ResumeLayout(false);
            this._tpUsage.PerformLayout();
            this._tpHelp.ResumeLayout(false);
            this._tpHelp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _lbNonTerminalName;
        private System.Windows.Forms.TextBox _tbNonTerminalName;
        private System.Windows.Forms.Button _btAddNonTerminal;
        private System.Windows.Forms.Button _btAddRule;
        private System.Windows.Forms.Label _lbInputRule;
        private System.Windows.Forms.TextBox _tbNewRule;
        private System.Windows.Forms.Label _lbPickNonTerminal;
        private System.Windows.Forms.ComboBox _cbNonTerminals;
        private System.Windows.Forms.Label _lbExistingRules;
        private System.Windows.Forms.ListBox _lbExistingRulesView;
        private System.Windows.Forms.Button _btDeleteRule;
        private System.Windows.Forms.Button _btBuildGrammar;
        private System.Windows.Forms.GroupBox _gbGrammarType;
        private System.Windows.Forms.RadioButton _rbContexFree;
        private System.Windows.Forms.RadioButton _rbRegular;
        private System.Windows.Forms.ListBox _lbResult;
        private System.Windows.Forms.CheckBox _chbCheck;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox _tbText;
        private System.Windows.Forms.TabControl _tcTabs;
        private System.Windows.Forms.TabPage _tpCreation;
        private System.Windows.Forms.TabPage _tpUsage;
        private System.Windows.Forms.TabPage _tpHelp;
        private System.Windows.Forms.Button _btCheckText;
        private System.Windows.Forms.Button _btAddLetters;
        private System.Windows.Forms.Button _btAddDigits;
        private System.Windows.Forms.Button _btAddCapitalLetters;
        private System.Windows.Forms.Button _btReset;
        private System.Windows.Forms.Label _lbInputText;
        private System.Windows.Forms.GroupBox _gbAutomatonType;
        private System.Windows.Forms.RadioButton _rbDFA;
        private System.Windows.Forms.RadioButton _rbDFAconverted;
        private System.Windows.Forms.RadioButton _rbNFA;
        private System.Windows.Forms.Button _btAgregate;
        private System.Windows.Forms.Button _btLoad;
        private System.Windows.Forms.Button _btSave;
        private System.Windows.Forms.Label _lbFilename;
        private System.Windows.Forms.TextBox _tbFilename;
        private System.Windows.Forms.TextBox _tbWorktime;
        private System.Windows.Forms.Label _lbWorktime;
        private System.Windows.Forms.Label _lbBuildTime;
        private System.Windows.Forms.TextBox _tbTimer;
        private System.Windows.Forms.TextBox _tbTransitionsCount;
        private System.Windows.Forms.Label _lbTransitionsCount;
        private System.Windows.Forms.Label _lbStatesCount;
        private System.Windows.Forms.TextBox _tbStatesCount;
    }
}

