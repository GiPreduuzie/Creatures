using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        public CreatureInfo(Membrane membrane)
        {
            InitializeComponent();

            var creature = (Creature) membrane.Creature;

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
                itemString.AppendLine(string.Format(format, item.Key, item.Value));

            return itemString.ToString();
        }
    }
}