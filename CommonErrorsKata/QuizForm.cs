﻿using CommonErrorsKata.Shared;
using System.IO;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonErrorsKata
{
    public partial class CommonErrorsForm : Form
    {
        private readonly AnswerQueue<TrueFalseAnswer> _answerQueue;
        private readonly string[] _files;
        private readonly SynchronizationContext _synchronizationContext;
        private int _time = 100;
        private string _visibleImagePath = null;
        private readonly string[] _possibleAnswers = null;
        private readonly int _maxAnswers = 15;

        public CommonErrorsForm()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
            _files = Directory.GetFiles(Environment.CurrentDirectory + @"..\..\..\ErrorPics");
            _possibleAnswers = _files.Select(f => Path.GetFileName(f)?.Replace(".png", " ")).ToArray();
            lstAnswers.DataSource = _possibleAnswers;
            _answerQueue = new AnswerQueue<TrueFalseAnswer>(_maxAnswers);
            Next();
            lstAnswers.Click += LstAnswers_Click;
            StartTimer();
        }
        private async void StartTimer()
        {
            await Task.Run(() =>
            {
                for (_time = 100; _time > 0; _time--)
                {
                    UpdateProgress(_time);
                    Thread.Sleep(50);
                }
                Message("Need to be quicker on your feet next time!  Try again...");
            });
        }

        private void LstAnswers_Click(object sender, EventArgs e)
        {
            _time = 100;
            var selected = _possibleAnswers[lstAnswers.SelectedIndex];
            var isCorrect = selected != null && selected == _visibleImagePath;
            _answerQueue.Enqueue(new TrueFalseAnswer(isCorrect));

            Next();
        }

        private void Next()
        {
            if (_answerQueue.Count == _maxAnswers && _answerQueue.Grade >= 98)
            {
                MessageBox.Show("Congratulations you've defeated me!");
                Application.Exit();
                return;
            }
            label1.Text = _answerQueue.Grade + "%";
            var file = _files.GetRandom();
            _visibleImagePath = Path.GetFileName(file)?.Replace(".png", " ");
            pbImage.ImageLocation = file;
        }

        public void UpdateProgress(int value)
        {
            _synchronizationContext.Post(new SendOrPostCallback(x =>
            {
                progress.Value = value;
            }), value);
        }
        public void Message(string value)
        {
            _synchronizationContext.Post(new SendOrPostCallback(x =>
            {
                MessageBox.Show(value);
            }), value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
