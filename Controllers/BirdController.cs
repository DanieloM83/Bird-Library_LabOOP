using BirdLab.Views;
using BirdLab.Models;
using BirdLab.Services;

namespace BirdLab.Controllers
{
    public class BirdController
    {
        private readonly BirdListView birdListView;
        private readonly BirdInfoView birdInfoView;
        private readonly BirdMenuView birdMenuView;
        private readonly BirdService birdService;
        private Bird? currentBird;

        public BirdController(BirdListView listView, BirdInfoView infoView, BirdMenuView menuView, BirdService service)
        {
            birdListView = listView;
            birdInfoView = infoView;
            birdMenuView = menuView;
            birdService = service;
            SubscribeToEvents();
            InitializeBirdList();
        }

        private void SubscribeToEvents()
        {
            birdListView.BirdSelected += OnBirdSelected;
            birdMenuView.AddBirdClicked += OnAddBirdClicked;
            birdMenuView.DeleteBirdClicked += OnDeleteBirdClicked;
        }

        private void InitializeBirdList()
        {
            try
            {
                var birds = birdService.GetAllBirds();
                birdListView.UpdateBirdList(birds);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading birds: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBirdSelected(object sender, Bird bird)
        {
            currentBird = bird;
            birdInfoView.UpdateBirdInfo(bird);
        }

        private void OnAddBirdClicked(object sender, (Bird bird, BirdDetails details) data)
        {
            try
            {
                // Use the transaction method from repository to add bird with details
                var repository = birdService.GetRepository(); // You'll need to expose this
                var success = repository.AddBirdWithDetailsAndHabitat(data.bird, data.details, null);
                
                if (success)
                {
                    UpdateBirdListView();
                    MessageBox.Show("Bird added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add bird.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding bird: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDeleteBirdClicked(object sender, EventArgs e)
        {
            if (currentBird == null)
            {
                MessageBox.Show("Please select a bird to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{currentBird.Name}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    birdService.DeleteBird(currentBird.Id);
                    UpdateBirdListView();
                    
                    // Clear the info view
                    var emptyBird = new Bird 
                    { 
                        Name = "Select a bird", 
                        Species = Species.Passerine,
                        BirdDetails = new BirdDetails { Info = "No bird selected" }
                    };
                    birdInfoView.UpdateBirdInfo(emptyBird);
                    currentBird = null;

                    MessageBox.Show("Bird deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting bird: {ex.Message}", "Database Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateBirdListView()
        {
            try
            {
                var birds = birdService.GetAllBirds();
                birdListView.UpdateBirdList(birds);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing bird list: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}