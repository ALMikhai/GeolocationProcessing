using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeolocationProcessing.SM
{
    class State
    {
        protected MainWindow _mainWindow;
        protected StateMachine _stateMachine;
        protected int _standartHeight = 40;
        protected int _standartMargine = 20;
        protected int _standartFontSize = 20;

        public State(MainWindow mainWindow, StateMachine stateMachine)
        {
            _mainWindow = mainWindow;
            _stateMachine = stateMachine;
        }

        public virtual void Enter() { }

        public virtual string GetDescription() { return ""; }

        public virtual int Process(double num, double maxNum) 
        {
            return 0;
        }

        public virtual void Exit() 
        {
            _mainWindow.ToolsPanel.Children.Clear();
        }
    }
}
