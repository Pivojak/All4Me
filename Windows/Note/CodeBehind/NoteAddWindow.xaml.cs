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

namespace All4Me
{
    /// <summary>
    /// Interakční logika pro NoteAddWindow.xaml
    /// </summary>
    public partial class NoteAddWindow : Window
    {
        /// <summary>
        /// Správce poznámkového bloku
        /// </summary>
        private NoteAdmin admin;
        /// <summary>
        /// Poznámka pro úpravu
        /// </summary>
        private NoteRecord record;
        /// <summary>
        /// Proměnná pro uložení akuální strany
        /// </summary>
        private byte selectPage;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="admin">Správce poznámkového bloku</param>
        /// <param name="record">Záznam pro úpravu</param>
        /// <param name="selectPage">Následující strana - tedy strana kde je místo</param>
        public NoteAddWindow(NoteAdmin admin, NoteRecord record, byte selectPage)
        {
            DataContext = record;
            InitializeComponent();
            this.admin = admin;
            this.record = record;
            this.selectPage = selectPage;
            if (record == null)
            {
                removeButton.Opacity = 0.5;
                removeButton.Focusable = false;
            }
                
        }


        /// <summary>
        /// Smaže vybranou poznámku - Tlačítko odebrání
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(record != null)
                {
                    admin.RemoveRecord(record);
                    admin.ConstructGraphicRecord(selectPage);
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        /// <summary>
        /// ULoží všechny změny - tlačítko uložení
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Nový záznam
            if(record == null)
            {
                try
                {
                    admin.EnterEditRecord(0, nameTextBox.Text, textTextBox.Text, null);
                    admin.ConstructGraphicRecord(selectPage);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            // Úprava stávajícího
            else
            {
                try
                {
                    admin.EnterEditRecord(1, nameTextBox.Text, textTextBox.Text, record);
                    admin.ConstructGraphicRecord(selectPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Close();
            
        }
    }
}
