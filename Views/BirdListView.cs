using BirdLab.Models;
using BirdLab.Themes;

namespace BirdLab.Views
{
    public class BirdListView : UserControl
    {
        public event EventHandler<Bird> BirdSelected;
        private readonly ListBox birdListBox = new();
        private List<Bird> birds = new();

        public BirdListView()
        {
            Dock = DockStyle.Fill;
            BackColor = CatppuccinMochaTheme.Crust;
            Padding = new Padding(5);
            BorderStyle = BorderStyle.FixedSingle;

            birdListBox.Dock = DockStyle.Fill;
            birdListBox.BackColor = CatppuccinMochaTheme.Mantle;
            birdListBox.ForeColor = CatppuccinMochaTheme.Text;
            birdListBox.Font = new Font("Inter", 12);
            birdListBox.BorderStyle = BorderStyle.None;
            birdListBox.ItemHeight = 50;
            birdListBox.IntegralHeight = false;
            birdListBox.DrawMode = DrawMode.OwnerDrawFixed;

            int hoverIndex = -1;

            birdListBox.DrawItem += (sender, e) => {
                if (e.Index < 0 || e.Index >= birds.Count) return;

                var bird = birds[e.Index];
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool isHovered = e.Index == hoverIndex;

                Color backgroundColor = isHovered ? CatppuccinMochaTheme.Surface0 : CatppuccinMochaTheme.Mantle;
                backgroundColor = isSelected ? CatppuccinMochaTheme.Surface2 : backgroundColor;

                using (Brush bgBrush = new SolidBrush(backgroundColor))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }

                // Draw bird name
                using (Brush textBrush = new SolidBrush(CatppuccinMochaTheme.Text))
                {
                    var nameFont = new Font("Segoe UI", 11, FontStyle.Bold);
                    e.Graphics.DrawString(bird.Name, nameFont, textBrush, e.Bounds.X + 10, e.Bounds.Y + 5);
                }

                // Draw species and additional info
                using (Brush subtextBrush = new SolidBrush(CatppuccinMochaTheme.Subtext0))
                {
                    var speciesFont = new Font("Segoe UI", 9, FontStyle.Italic);
                    var speciesText = $"{bird.Species}";
                    
                    // Add observation count if available
                    if (bird.Observations != null && bird.Observations.Any())
                    {
                        speciesText += $" • {bird.Observations.Count} observations";
                    }

                    // Add endangered status
                    if (bird.BirdDetails?.IsEndangered == true)
                    {
                        speciesText += " • ⚠️ Endangered";
                    }

                    e.Graphics.DrawString(speciesText, speciesFont, subtextBrush, e.Bounds.X + 10, e.Bounds.Y + 25);
                }
            };

            birdListBox.SelectedIndexChanged += (s, e) => OnBirdSelected();
            birdListBox.MouseMove += (s, e) => {
                int index = birdListBox.IndexFromPoint(e.Location);
                if (index != hoverIndex)
                {
                    hoverIndex = index;
                    birdListBox.Invalidate();
                }
            };

            birdListBox.MouseLeave += (s, e) => {
                hoverIndex = -1;
                birdListBox.Invalidate();
            };

            Controls.Add(birdListBox);
        }

        private void OnBirdSelected()
        {
            if (birdListBox.SelectedIndex >= 0 && birdListBox.SelectedIndex < birds.Count)
            {
                var selectedBird = birds[birdListBox.SelectedIndex];
                BirdSelected?.Invoke(this, selectedBird);
            }
        }

        public void UpdateBirdList(List<Bird> newBirds)
        {
            birds = newBirds ?? new List<Bird>();
            birdListBox.Items.Clear();
            
            // Add items (we'll use custom drawing, so the actual item text doesn't matter much)
            for (int i = 0; i < birds.Count; i++)
            {
                birdListBox.Items.Add($"{birds[i].Name}");
            }
        }
    }
}