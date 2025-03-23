using BirdLab.Models;
using BirdLab.Themes;

namespace BirdLab.Views
{

    public class BirdInfoView : UserControl
    {
        private readonly Label nameLabel = new Label();
        private readonly Label speciesLabel = new Label();
        private readonly Label infoLabel = new Label();
        private readonly Panel cardPanel = new Panel();

        public BirdInfoView()
        {
            Dock = DockStyle.Fill;
            BackColor = CatppuccinMochaTheme.Mantle;
            Size = new Size(350, 249);
            Padding = new Padding(1);

            SetupLabel(nameLabel, FontStyle.Bold, 14);
            SetupLabel(speciesLabel, FontStyle.Italic, 12, CatppuccinMochaTheme.Subtext0);
            SetupLabel(infoLabel, FontStyle.Regular, 12, CatppuccinMochaTheme.Subtext1);
            
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.BackColor = CatppuccinMochaTheme.Base;
            cardPanel.Padding = new Padding(5);
            
            nameLabel.Location = new Point(10, 10);
            speciesLabel.Location = new Point(10, 40);
            infoLabel.Location = new Point(10, 70);

            cardPanel.Controls.AddRange([nameLabel, speciesLabel, infoLabel]);
            Controls.Add(cardPanel);
        }

        private void SetupLabel(Label label, FontStyle style, int fontSize, Color? color = null)
        {
            label.Font = new Font("Segoe UI", fontSize, style);
            label.ForeColor = color ?? CatppuccinMochaTheme.Text;
            label.AutoSize = true;
        }

        public void UpdateBirdInfo(BirdDTO bird)
        {
            cardPanel.Visible = false;
            nameLabel.Text = $"Name: {bird.Name}";
            speciesLabel.Text = $"Species: {bird.Species}";
            infoLabel.Text = $"Info: {bird.Info}";
            cardPanel.Visible = true;
        }
    }
}
