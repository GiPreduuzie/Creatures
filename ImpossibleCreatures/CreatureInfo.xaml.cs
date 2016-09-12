using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using CellsAutomate;
using CellsAutomate.Creatures;
using Creatures.Language.Parsers;

namespace ImpossibleCreatures
{
    /// <summary>
    /// Interaction logic for CreatureInfo.xaml
    /// </summary>
    public partial class CreatureInfo : Window
    {
        private bool _isClosed = false;

        public CreatureInfo(Membrane membrane, bool easyClosing)
        {
            InitializeComponent();

            if (easyClosing)
            {
                Deactivated += Window_Deactivated;
                Closing += CreatureInfo_Closing;
            }

            var creature = (Creature)membrane.Creature;

            Location.Content = $"({membrane.Position.X}, {membrane.Position.Y})";
            Energy.Content = membrane.EnergyPoints;
            Age.Content = membrane.Age;
            Generation.Content = membrane.Generation;
            GenLength.Content = creature.GenotypeLength;

            ActionCode.Text = new CommandPrinter().ParseCommands(creature.CommandsForGetAction);
            Memory.Text = DictToString(creature.CreatureMemory);
            ReceivedMessages.Text = string.Join(Environment.NewLine, creature.ReceivedMessages);
        }



        public string DictToString<T, V>(IEnumerable<KeyValuePair<T, V>> items)
        {
            var format = "{0}='{1}'";

            StringBuilder itemString = new StringBuilder();
            foreach (var item in items)
                itemString.AppendLine(string.Format(format, item.Key, item));

            return itemString.ToString();
        }

        private void CreatureInfo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isClosed = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!_isClosed)
            {
                Close();
            }
        }
    }
}