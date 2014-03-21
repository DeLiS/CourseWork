using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Analyzer.Forms.Properties;
using FiniteStateMachines.Grammars;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;
namespace Analyzer.Forms
{
    class Controller
    {
        private List<String> NonTerminals { get; set; }
        private SortedDictionary<String, RegularGrammarRule<int>> _rules = new SortedDictionary<string, RegularGrammarRule<int>>();
        private readonly NumberGenerator _generator = new NumberGenerator();
        private SortedSet<ISymbol<string>> _filter = new SortedSet<ISymbol<string>>();
        private RegularGrammar<int> _grammar;

        private readonly MainForm _form;
        private Button _btReset;
        private ComboBox _nonTerminalsList;
        private TextBox _nonTerminalName;
        private TextBox _newRule;
        private TextBox _tbTransitionsCount;
        private TextBox _tbStatesCount;
        private ListBox _existingRulesView;
        private TextBox _tbTimer;
        private RadioButton _regular;
        private RadioButton _contextFree;
        private Button _btAgregate;
        private RadioButton _rbNFA;
        private RadioButton _rbDFA;
        private RadioButton _rbDFAconverted;
        private TextBox _tbFilename;
        private Button _btSave;
        private Button _btLoad;

        private Button _btbuildgrammar;
        private ListBox _lbResult;
        private CheckBox _chbCheck;
        private TextBox _tbWorktime;

        private TextBox _tbText;
        private Button _btCheckText;
        private Button _btAddLetters;
        private Button _btAddDigits;
        private Button _btAddCapitalLetters;

        public Controller(MainForm form)
        {
            NonTerminals = new List<string>();
            _form = form;
            InitializeControls();

        }

        private void InitializeControls()
        {
            var controls = _form.Controls;
            foreach (var control in controls)
            {
                var ascontrol = control as Control;
                if (ascontrol == null)
                    continue;
                if (ascontrol.Name != "_tcTabs")
                    continue;

                var tbTabs = ascontrol as TabControl;
                var tabPages = tbTabs.Controls;
                foreach (var tabPage in tabPages)
                {
                    var tp = tabPage as TabPage;
                    var tpControls = tp.Controls;
                    foreach (var something in tpControls)
                    {

                        var tabControl = something as Control;
                        switch (tabControl.Name)
                        {
                            case "_tbFilename":
                                _tbFilename = tabControl as TextBox;
                                break;
                            case "_btSave":
                                _btSave = tabControl as Button;
                                _btSave.Click += BtSaveClick;
                                break;
                            case "_btLoad":
                                _btLoad = tabControl as Button;
                                _btLoad.Click += BtLoadClick;
                                break;
                            case "_btAgregate":
                                _btAgregate = tabControl as Button;
                                _btAgregate.Click += OnAgragateClick;
                                break;
                            case "_tbTimer":
                                _tbTimer = tabControl as TextBox;
                                break;
                            case "_btReset":
                                _btReset = tabControl as Button;
                                _btReset.Click += BtResetClick;
                                break;
                            case "_cbNonTerminals":
                                _nonTerminalsList = tabControl as ComboBox;
                                _nonTerminalsList.SelectedIndexChanged += CbRuleSelected;
                                break;
                            case "_tbNonTerminalName":
                                _nonTerminalName = tabControl as TextBox;

                                break;
                            case "_tbStatesCount":
                                _tbStatesCount = tabControl as TextBox;
                                break;
                            case "_tbTransitionsCount":
                                _tbTransitionsCount = tabControl as TextBox;
                                break;
                            case "_btAddNonTerminal":
                                var buttonAddNonTerminal = tabControl as Button;
                                buttonAddNonTerminal.Click += BtAddNonTerminalClick;
                                break;
                            case "_btAddRule":
                                var buttonAddRule = tabControl as Button;
                                buttonAddRule.Click += BtAddRuleClick;
                                break;
                            case "_tbNewRule":
                                _newRule = tabControl as TextBox;
                                break;
                            case "_lbExistingRulesView":
                                _existingRulesView = tabControl as ListBox;
                                break;
                            case "_btDeleteRule":
                                var btdeleterule = tabControl as Button;
                                btdeleterule.Click += BtDeleteRuleClick;
                                break;
                            case "_btBuildGrammar":
                                _btbuildgrammar = tabControl as Button;
                                _btbuildgrammar.Click += BtBuildGrammarClick;
                                _btbuildgrammar.Enabled = false;
                                break;
                            case "_btCheckText":
                                _btCheckText = tabControl as Button;
                                _btCheckText.Click += BtNewCheckTextClick;
                                _btCheckText.Enabled = false;
                                break;
                            case "_btAddLetters":
                                _btAddLetters = tabControl as Button;
                                _btAddLetters.Click += BtAddSmallLettersClick;
                                _btAddLetters.Enabled = false;
                                break;
                            case "_btAddCapitalLetters":
                                _btAddCapitalLetters = tabControl as Button;
                                _btAddCapitalLetters.Click += BtAddCapitalLettersClick;
                                _btAddCapitalLetters.Enabled = false;
                                break;
                            case "_btAddDigits":
                                _btAddDigits = tabControl as Button;
                                _btAddDigits.Click += BtAddDigitsClick;
                                _btAddDigits.Enabled = false;
                                break;
                            case "_tbText":
                                _tbText = tabControl as TextBox;
                                break;
                            case "_lbResult":
                                _lbResult = tabControl as ListBox;
                                break;
                            case "_tbWorktime":
                                _tbWorktime = tabControl as TextBox;
                                break;
                            case "_gbGrammarType":
                                var groupbox = tabControl as GroupBox;
                                for (int i = 0; i < groupbox.Controls.Count; ++i)
                                {
                                    switch (groupbox.Controls[i].Name)
                                    {
                                        case "_rbRegular":
                                            _regular = groupbox.Controls[i] as RadioButton;
                                            _regular.CheckedChanged += RbTypeCheckedChanged;
                                            break;
                                        case "_rbContexFree":
                                            _contextFree = groupbox.Controls[i] as RadioButton;
                                            _contextFree.CheckedChanged += RbTypeCheckedChanged;
                                            break;
                                    }
                                }
                                _regular.Checked = true;
                                break;
                            case "_gbAutomatonType":
                                var groupbox2 = tabControl as GroupBox;
                                for (int i = 0; i < groupbox2.Controls.Count; ++i)
                                {
                                    switch (groupbox2.Controls[i].Name)
                                    {
                                        case "_rbNFA":
                                            _rbNFA = groupbox2.Controls[i] as RadioButton;
                                            break;
                                        case "_rbDFA":
                                            _rbDFA = groupbox2.Controls[i] as RadioButton;
                                            break;
                                        case "_rbDFAconverted":
                                            _rbDFAconverted = groupbox2.Controls[i] as RadioButton;
                                            break;

                                    }
                                }
                                break;
                            case "_chbCheck":
                                _chbCheck = tabControl as CheckBox;
                                _chbCheck.Checked = true;
                                _chbCheck.CheckedChanged += CheckChanged;
                                _chbCheck.Enabled = false;
                                break;
                        }
                    }
                }
            }
        }

        private void BtLoadClick(object sender, EventArgs e)
        {
            var filename = _tbFilename.Text;
            Reset();
            using (var streamReader = new StreamReader(filename))
            {
                var type = streamReader.ReadLine();
                if (type == "regular")
                {
                    _regular.Checked = true;
                }
                else
                {
                    _contextFree.Checked = true;
                }
                int nonTerminalsCount = Convert.ToInt32(streamReader.ReadLine());
                for (int i = 0; i < nonTerminalsCount; ++i)
                {
                    var nonTerminal = streamReader.ReadLine();
                    _nonTerminalName.Text = nonTerminal;
                    BtAddNonTerminalClick(null,null);
                    _nonTerminalsList.SelectedItem = nonTerminal;
                    int sequencesCount = Convert.ToInt32(streamReader.ReadLine());
                    for(int j=0;j<sequencesCount;++j)
                    {
                        var sequence = streamReader.ReadLine();
                        _newRule.Text = sequence;
                        BtAddRuleClick(null,null);
                    }
                }
            }
        }

        private void BtSaveClick(object sender, EventArgs e)
        {
            var filename = _tbFilename.Text;
            using (var streamWriter = new StreamWriter(filename))
            {
                streamWriter.WriteLine(_regular.Checked ? "regular" : "contextfree");
                streamWriter.WriteLine(NonTerminals.Count);
                foreach (var nonTerminal in NonTerminals)
                {
                    streamWriter.WriteLine(nonTerminal);
                    var sequences = _rules[nonTerminal].Sequences;
                    streamWriter.WriteLine(sequences.Count);
                    foreach (var sequence in sequences)
                    {
                        streamWriter.WriteLine(sequence);
                    }
                }
            }
        }

        private void OnAgragateClick(object sender, EventArgs e)
        {
            var nonTerminal = _nonTerminalsList.SelectedItem as String;
            _rules[nonTerminal].AgregateRules();
            CbRuleSelected(null, null);
        }
        private void BtResetClick(object sender, EventArgs e)
        {
            Reset();
        }
        private void BtAddNonTerminalClick(object sender, EventArgs e)
        {

            string nonTerminalName = _nonTerminalName.Text;
            if (NonTerminals.Contains(nonTerminalName))
            {
                MessageBox.Show(Resources.Controller_BtAddNonTerminalClick_ThatNonTerminalIsAlreadyCreated);
                return;
            }
            _regular.Enabled = false;
            _contextFree.Enabled = false;
            if (_regular.Checked)
                _rules[nonTerminalName] = new RegularGrammarRule<int>(nonTerminalName, _generator);
            else
            {
                _rules[nonTerminalName] = new ContextFreeGrammarRule<int, int>(nonTerminalName, _generator, _generator);
            }
            NonTerminals.Add(nonTerminalName);
            // _blist = new BindingList<string>(NonTerminals);
            _nonTerminalsList.Items.Clear();
            _nonTerminalsList.Items.AddRange(NonTerminals.ToArray());
            // _nonTerminalsList.DataSource = _blist;
        }
        private void BtAddRuleClick(object sender, EventArgs e)
        {
            var nonTerminal = _nonTerminalsList.SelectedItem as String;
            var rule = _newRule.Text;
            _rules[nonTerminal].AddSequence(rule);
            _btbuildgrammar.Enabled = true;
            CbRuleSelected(null, null);

        }
        private void CbRuleSelected(object sender, EventArgs e)
        {
            var nonTerminal = _nonTerminalsList.SelectedItem as String;
            var sequences = _rules[nonTerminal].Sequences;
            _existingRulesView.Items.Clear();
            foreach (var sequence in sequences)
            {
                _existingRulesView.Items.Add(sequence);
            }
            _chbCheck.Enabled = true;
            _chbCheck.Checked = !_filter.Contains(new Symbol<string>(nonTerminal, SymbolType.NonTerminal));
            _btAddLetters.Enabled = true;
            _btAddDigits.Enabled = true;
            _btAddCapitalLetters.Enabled = true;
        }
        private void BtDeleteRuleClick(object sender, EventArgs e)
        {
            var sequence = _existingRulesView.SelectedItem as String;
            var nonterminal = _nonTerminalsList.SelectedItem as String;
            _rules[nonterminal].Sequences.Remove(sequence);
            CbRuleSelected(null, null);
            // _btCheck.Enabled = false;
            _btCheckText.Enabled = false;
            _btbuildgrammar.Enabled = true;
        }
        private void BtBuildGrammarClick(object sender, EventArgs e)
        {
            _tbTransitionsCount.Clear();
            _tbStatesCount.Clear();
            _grammar = _regular.Checked ? new RegularGrammar<int>(_generator) : new ContextFreeGrammar<int, int>(_generator, _generator);
            foreach (var grammarRule in _rules)
            {
                _grammar.AddRule(grammarRule.Value);
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _grammar.BuildAcceptorForEachRule(_rbDFA.Checked);

            if (_rbDFAconverted.Checked || _rbDFA.Checked)
            {
                _grammar.MakeAcceptorsDeterminate();
            }
            stopwatch.Stop();
            _tbTimer.Text = stopwatch.Elapsed.ToString();
            // _btCheck.Enabled = true;
            _btCheckText.Enabled = true;
            int statesCount = 0;
            int transtionsCount = 0;
            foreach (var regularGrammarRule in _rules)
            {
                statesCount += regularGrammarRule.Value.Acceptor.TotalStates;
                transtionsCount += regularGrammarRule.Value.Acceptor.TransitionsCount;
            }

            _tbStatesCount.Text = statesCount.ToString();
            _tbTransitionsCount.Text = transtionsCount.ToString();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            var nonTerminal = _nonTerminalsList.SelectedItem as String;
            if (_chbCheck.Checked)
            {
                _filter.Remove(new Symbol<string>(nonTerminal, SymbolType.NonTerminal));
            }
            else
            {
                _filter.Add(new Symbol<string>(nonTerminal, SymbolType.NonTerminal));
            }
        }

        private void BtAddSmallLettersClick(object sender, EventArgs e)
        {
            for (char c = 'a'; c <= 'z'; ++c)
            {
                _newRule.Text = String.Format("'{0}'", c);
                BtAddRuleClick(null, null);
            }

        }
        private void BtAddDigitsClick(object sender, EventArgs e)
        {
            for (int i = 0; i <= 9; ++i)
            {
                _newRule.Text = String.Format("'{0}'", i);
                BtAddRuleClick(null, null);
            }
        }
        private void BtAddCapitalLettersClick(object sender, EventArgs e)
        {
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                _newRule.Text = String.Format("'{0}'", c);
                BtAddRuleClick(null, null);
            }

        }

        private void RbTypeCheckedChanged(object sender, EventArgs e)
        {
            if (_regular.Checked)
            {
                _rbDFA.Visible = _rbDFAconverted.Visible = _rbNFA.Visible = true;
                _rbNFA.Checked = true;
            }
            else
            {
                _rbDFA.Visible = false;
                _rbDFAconverted.Visible = false;
                _rbNFA.Checked = true;
            }

        }

        private void Reset()
        {
            _rules = new SortedDictionary<string, RegularGrammarRule<int>>();
            _filter = new SortedSet<ISymbol<string>>();
            _existingRulesView.Items.Clear();
            _nonTerminalsList.Items.Clear();
            _regular.Enabled = true;
            _contextFree.Enabled = true;
            _regular.Checked = true;
            _grammar = null;
            _tbText.Clear();
            _btbuildgrammar.Enabled = false;
            _tbStatesCount.Clear();
            _tbTransitionsCount.Clear();
            _tbText.Clear();
            _tbFilename.Clear();
            _newRule.Clear();
            _nonTerminalName.Clear();
            _nonTerminalsList.Text = String.Empty;
            _tbTimer.Clear();
            NonTerminals.Clear();
        }
        private void BtNewCheckTextClick(object sender, EventArgs e)
        {
            string text = _tbText.Text;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = _grammar.CheckText(text);
            stopwatch.Stop();
            _tbWorktime.Text = stopwatch.Elapsed.ToString();
            _lbResult.Items.Clear();
            foreach (var pair in result)
            {
                if (!_filter.Contains(pair.Value))
                {
                    _lbResult.Items.Add(String.Format("{0} - {1}",
                                                      text.Substring(pair.Key.Key, pair.Key.Value - pair.Key.Key + 1),
                                                      pair.Value.Value));
                }
            }
        }
    }
}
