using BirdLab.Models;
using BirdLab.Themes;

namespace BirdLab.Views
{
    public class BirdInfoView : UserControl
    {
        private readonly Label nameLabel = new Label();
        private readonly Label speciesLabel = new Label();
        private readonly Label infoLabel = new Label();
        private readonly Label descriptionLabel = new Label();
        private readonly Label physicalLabel = new Label();
        private readonly Label dietLabel = new Label();
        private readonly Label endangeredLabel = new Label();
        private readonly Label observationsLabel = new Label();
        private readonly Panel cardPanel = new Panel();

        public BirdInfoView()
        {
            Dock = DockStyle.Fill;
            BackColor = CatppuccinMochaTheme.Mantle;
            Size = new Size(350, 400);
            Padding = new Padding(1);

            SetupLabel(nameLabel, FontStyle.Bold, 16);
            SetupLabel(speciesLabel, FontStyle.Italic, 12, CatppuccinMochaTheme.Subtext0);
            SetupLabel(infoLabel, FontStyle.Regular, 11, CatppuccinMochaTheme.Subtext1);
            SetupLabel(descriptionLabel, FontStyle.Regular, 11, CatppuccinMochaTheme.Text);
            SetupLabel(physicalLabel, FontStyle.Regular, 10, CatppuccinMochaTheme.Subtext0);
            SetupLabel(dietLabel, FontStyle.Regular, 10, CatppuccinMochaTheme.Subtext0);
            SetupLabel(endangeredLabel, FontStyle.Bold, 10, CatppuccinMochaTheme.Red);
            SetupLabel(observationsLabel, FontStyle.Regular, 10, CatppuccinMochaTheme.Blue);

            cardPanel.Dock = DockStyle.Fill;
            cardPanel.BackColor = CatppuccinMochaTheme.Base;
            cardPanel.Padding = new Padding(5);
            cardPanel.AutoScroll = true;

            // Position labels
            nameLabel.Location = new Point(10, 10);
            speciesLabel.Location = new Point(10, 35);
            infoLabel.Location = new Point(10, 60);
            descriptionLabel.Location = new Point(10, 90);
            physicalLabel.Location = new Point(10, 130);
            dietLabel.Location = new Point(10, 150);
            endangeredLabel.Location = new Point(10, 170);
            observationsLabel.Location = new Point(10, 200);

            cardPanel.Controls.AddRange([nameLabel, speciesLabel, infoLabel, descriptionLabel, 
                                       physicalLabel, dietLabel, endangeredLabel, observationsLabel]);
            Controls.Add(cardPanel);
        }

        private void SetupLabel(Label label, FontStyle style, int fontSize, Color? color = null)
        {
            label.Font = new Font("Segoe UI", fontSize, style);
            label.ForeColor = color ?? CatppuccinMochaTheme.Text;
            label.AutoSize = true;
            label.MaximumSize = new Size(320, 0);
        }

        public void UpdateBirdInfo(Bird bird)
        {
            cardPanel.Visible = false;

            nameLabel.Text = $"Name: {bird.Name}";
            speciesLabel.Text = $"Species: {bird.Species}";

            // Handle BirdDetails
            if (bird.BirdDetails != null)
            {
                infoLabel.Text = $"Info: {bird.BirdDetails.Info}";
                descriptionLabel.Text = !string.IsNullOrEmpty(bird.BirdDetails.Description) 
                    ? $"Description: {bird.BirdDetails.Description}" 
                    : "Description: Not available";

                var physicalInfo = "";
                if (bird.BirdDetails.AverageLength.HasValue)
                    physicalInfo += $"Length: {bird.BirdDetails.AverageLength:F1}cm ";
                if (bird.BirdDetails.AverageWeight.HasValue)
                    physicalInfo += $"Weight: {bird.BirdDetails.AverageWeight:F1}g";
                
                physicalLabel.Text = !string.IsNullOrEmpty(physicalInfo) ? physicalInfo : "Physical data: Not available";
                dietLabel.Text = !string.IsNullOrEmpty(bird.BirdDetails.Diet) 
                    ? $"Diet: {bird.BirdDetails.Diet}" 
                    : "Diet: Not specified";

                endangeredLabel.Text = bird.BirdDetails.IsEndangered ? "⚠️ ENDANGERED SPECIES" : "";
                endangeredLabel.Visible = bird.BirdDetails.IsEndangered;
            }
            else
            {
                infoLabel.Text = "Info: Not available";
                descriptionLabel.Text = "Description: Not available";
                physicalLabel.Text = "Physical data: Not available";
                dietLabel.Text = "Diet: Not specified";
                endangeredLabel.Visible = false;
            }

            // Handle Observations
            var observationCount = bird.Observations?.Count ?? 0;
            observationsLabel.Text = $"Observations: {observationCount} recorded";

            cardPanel.Visible = true;
        }
    }
}