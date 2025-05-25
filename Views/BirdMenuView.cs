using BirdLab.Models;
using BirdLab.Themes;

namespace BirdLab.Views
{
    public class BirdMenuView : UserControl
    {
        public event EventHandler<(Bird bird, BirdDetails details)> AddBirdClicked;
        public event EventHandler DeleteBirdClicked;

        private readonly Button addBirdButton = new() { Text = "Add" };
        private readonly Button deleteBirdButton = new() { Text = "Del" };
        private readonly TextBox nameTextBox = new() { PlaceholderText = "Enter bird name..." };
        private readonly TextBox infoTextBox = new() { PlaceholderText = "Enter bird info..." };
        private readonly TextBox descriptionTextBox = new() { PlaceholderText = "Enter description..." };
        private readonly TextBox dietTextBox = new() { PlaceholderText = "Enter diet..." };
        private readonly NumericUpDown lengthNumeric = new();
        private readonly NumericUpDown weightNumeric = new();
        private readonly CheckBox endangeredCheckBox = new() { Text = "Endangered Species" };
        private readonly ComboBox speciesComboBox = new();

        public BirdMenuView()
        {
            BackColor = CatppuccinMochaTheme.Mantle;
            Padding = new Padding(15);
            Size = new Size(350, 500);
            AutoScroll = true;

            speciesComboBox.DataSource = Enum.GetValues(typeof(Species));
            speciesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // Setup numeric controls
            lengthNumeric.DecimalPlaces = 1;
            lengthNumeric.Minimum = 0;
            lengthNumeric.Maximum = 500;
            lengthNumeric.Value = 0;

            weightNumeric.DecimalPlaces = 1;
            weightNumeric.Minimum = 0;
            weightNumeric.Maximum = 50000;
            weightNumeric.Value = 0;

            StyleButton(addBirdButton, CatppuccinMochaTheme.Green);
            StyleButton(deleteBirdButton, CatppuccinMochaTheme.Red);

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Style all input controls
            StyleTextBox(nameTextBox);
            StyleTextBox(infoTextBox);
            StyleTextBox(descriptionTextBox);
            StyleTextBox(dietTextBox);
            StyleComboBox(speciesComboBox);
            StyleNumericUpDown(lengthNumeric);
            StyleNumericUpDown(weightNumeric);
            StyleCheckBox(endangeredCheckBox);

            // Add labels and controls
            layout.Controls.Add(new Label { Text = "Bird Name:", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(nameTextBox);
            
            layout.Controls.Add(new Label { Text = "Species:", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(speciesComboBox);
            
            layout.Controls.Add(new Label { Text = "Basic Info:", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(infoTextBox);
            
            layout.Controls.Add(new Label { Text = "Description:", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(descriptionTextBox);
            
            layout.Controls.Add(new Label { Text = "Diet:", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(dietTextBox);
            
            layout.Controls.Add(new Label { Text = "Length (cm):", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(lengthNumeric);
            
            layout.Controls.Add(new Label { Text = "Weight (g):", ForeColor = CatppuccinMochaTheme.Text, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            layout.Controls.Add(weightNumeric);
            
            layout.Controls.Add(endangeredCheckBox);
            layout.Controls.Add(addBirdButton);
            layout.Controls.Add(deleteBirdButton);

            Controls.Add(layout);

            // Set consistent widths
            var controlWidth = 280;
            nameTextBox.Width = infoTextBox.Width = descriptionTextBox.Width = dietTextBox.Width = 
            speciesComboBox.Width = lengthNumeric.Width = weightNumeric.Width = 
            addBirdButton.Width = deleteBirdButton.Width = controlWidth;

            addBirdButton.Click += (s, e) => OnAddBirdClicked();
            deleteBirdButton.Click += (s, e) => DeleteBirdClicked?.Invoke(this, EventArgs.Empty);
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = CatppuccinMochaTheme.Base;
            textBox.ForeColor = CatppuccinMochaTheme.Text;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Consolas", 12, FontStyle.Regular);
                        textBox.Height = 35;
            textBox.Margin = new Padding(0, 0, 0, 10);
        }

        private void StyleComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = CatppuccinMochaTheme.Base;
            comboBox.ForeColor = CatppuccinMochaTheme.Text;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font("Consolas", 12, FontStyle.Regular);
            comboBox.Height = 35;
            comboBox.Margin = new Padding(0, 0, 0, 10);
        }

        private void StyleNumericUpDown(NumericUpDown numeric)
        {
            numeric.BackColor = CatppuccinMochaTheme.Base;
            numeric.ForeColor = CatppuccinMochaTheme.Text;
            numeric.BorderStyle = BorderStyle.None;
            numeric.Font = new Font("Consolas", 12, FontStyle.Regular);
            numeric.Height = 35;
            numeric.Margin = new Padding(0, 0, 0, 10);
        }

        private void StyleCheckBox(CheckBox checkBox)
        {
            checkBox.ForeColor = CatppuccinMochaTheme.Text;
            checkBox.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            checkBox.Margin = new Padding(0, 5, 0, 15);
        }

        private void OnAddBirdClicked()
        {
            var name = nameTextBox.Text.Trim();
            var info = infoTextBox.Text.Trim();
            var species = (Species)speciesComboBox.SelectedItem;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(info))
            {
                MessageBox.Show("Please enter both bird name and basic info.", "Missing Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newBird = new Bird
            {
                Name = name,
                Species = species,
                CreatedAt = DateTime.UtcNow
            };

            var newBirdDetails = new BirdDetails
            {
                Info = info,
                Description = !string.IsNullOrEmpty(descriptionTextBox.Text.Trim()) ? descriptionTextBox.Text.Trim() : null,
                Diet = !string.IsNullOrEmpty(dietTextBox.Text.Trim()) ? dietTextBox.Text.Trim() : null,
                AverageLength = lengthNumeric.Value > 0 ? (double?)lengthNumeric.Value : null,
                AverageWeight = weightNumeric.Value > 0 ? (double?)weightNumeric.Value : null,
                IsEndangered = endangeredCheckBox.Checked
            };

            AddBirdClicked?.Invoke(this, (newBird, newBirdDetails));
            ClearForm();
        }

        private void ClearForm()
        {
            nameTextBox.Clear();
            infoTextBox.Clear();
            descriptionTextBox.Clear();
            dietTextBox.Clear();
            lengthNumeric.Value = 0;
            weightNumeric.Value = 0;
            endangeredCheckBox.Checked = false;
            speciesComboBox.SelectedIndex = 0;
        }

        private void StyleButton(Button button, Color bgColor, Color? textColor = null)
        {
            button.BackColor = bgColor;
            button.ForeColor = textColor ?? CatppuccinMochaTheme.Mantle;
            button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Height = 45;
            button.Margin = new Padding(0, 5, 0, 5);
        }
    }
}