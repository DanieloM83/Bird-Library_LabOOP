using BirdLab.Models;

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
            BackColor = Color.White;
            Padding = new Padding(15);
            Size = new Size(300, 200);

            InitializeCardPanel();
            InitializeLabels();
            ArrangeControls();
        }

        private void InitializeCardPanel()
        {
            cardPanel.Size = new Size(280, 160);
            cardPanel.BackColor = Color.White;
            cardPanel.Padding = new Padding(15);
            cardPanel.Location = new Point(10, 10);
            cardPanel.Paint += (sender, e) => DrawCardShadow(e.Graphics);
        }

        private void DrawCardShadow(Graphics graphics)
        {
            using (Pen shadowPen = new Pen(Color.LightGray, 3))
            {
                graphics.DrawRectangle(shadowPen, 0, 0, cardPanel.Width - 1, cardPanel.Height - 1);
            }
        }

        private void InitializeLabels()
        {
            SetupLabel(nameLabel, FontStyle.Bold, 14, Color.DarkSlateBlue);
            SetupLabel(speciesLabel, FontStyle.Italic, 12, Color.DarkGreen);
            SetupLabel(infoLabel, FontStyle.Regular, 12, Color.Black);
        }

        private void ArrangeControls()
        {
            nameLabel.Location = new Point(10, 10);
            speciesLabel.Location = new Point(10, 40);
            infoLabel.Location = new Point(10, 70);

            cardPanel.Controls.AddRange(new Control[] { nameLabel, speciesLabel, infoLabel });
            Controls.Add(cardPanel);
        }

        private void SetupLabel(Label label, FontStyle style, int fontSize, Color color)
        {
            label.Font = new Font("Segoe UI", fontSize, style);
            label.ForeColor = color;
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
