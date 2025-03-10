using BirdLab.Models;

namespace BirdLab.Views
{

    public class BirdMenuView : UserControl
    {
        public event EventHandler<BirdDTO> AddBirdClicked;
        public event EventHandler DeleteBirdClicked;

        private readonly Button addBirdButton = new() { Text = "Add Bird" };
        private readonly Button deleteBirdButton = new() { Text = "Delete Bird" };
        private readonly TextBox nameTextBox = new() { PlaceholderText = "Enter bird name..." };
        private readonly TextBox infoTextBox = new() { PlaceholderText = "Enter bird description..." };
        private readonly ComboBox speciesComboBox = new();

        public BirdMenuView()
        {
            BackColor = Color.White;
            Padding = new Padding(15);
            Size = new Size(320, 300);

            speciesComboBox.DataSource = Enum.GetValues(typeof(Species));
            speciesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            StyleButton(addBirdButton, Color.MediumSeaGreen, Color.White);
            StyleButton(deleteBirdButton, Color.Crimson, Color.White);

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var controls = new Control[] { nameTextBox, infoTextBox, speciesComboBox, addBirdButton, deleteBirdButton };
            layout.Controls.AddRange(controls);
            Controls.Add(layout);

            nameTextBox.Height = infoTextBox.Height = speciesComboBox.Height = 40;
            addBirdButton.Height = deleteBirdButton.Height = 50;

            nameTextBox.Width = infoTextBox.Width = speciesComboBox.Width = addBirdButton.Width = deleteBirdButton.Width = 280;

            addBirdButton.Click += (s, e) => OnAddBirdClicked();
            deleteBirdButton.Click += (s, e) => DeleteBirdClicked?.Invoke(this, null);
        }

        private void OnAddBirdClicked()
        {
            var name = nameTextBox.Text;
            var info = infoTextBox.Text;
            var species = (Species)speciesComboBox.SelectedItem;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(info))
            {
                MessageBox.Show("Please enter both name and description.");
                return;
            }

            var newBird = new BirdDTO
            {
                Id = new Random().Next(1, 1000),
                Name = name,
                Species = species,
                Info = info
            };

            AddBirdClicked?.Invoke(this, newBird);
        }

        private void StyleButton(Button button, Color bgColor, Color textColor)
        {
            button.BackColor = bgColor;
            button.ForeColor = textColor;
            button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Margin = new Padding(5);
        }
    }
}
