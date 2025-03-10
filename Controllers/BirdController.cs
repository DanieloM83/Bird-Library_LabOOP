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

        private BirdDTO? currentBird;

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
            birdListView.UpdateBirdList(birdService.GetAllBirds());
        }

        private void OnBirdSelected(object sender, BirdDTO bird)
        {
            currentBird = bird;
            birdInfoView.UpdateBirdInfo(bird);
        }

        private void OnAddBirdClicked(object sender, BirdDTO newBird)
        {
            birdService.AddBird(newBird);
            UpdateBirdListView();
        }

        private void OnDeleteBirdClicked(object sender, EventArgs e)
        {
            if (currentBird != null)
            {
                birdService.DeleteBird(currentBird.Id);
                UpdateBirdListView();
                birdInfoView.UpdateBirdInfo(new BirdDTO { Id = -1, Name = "None", Species = Species.Penguin, Info = "None" });
            }
        }

        private void UpdateBirdListView()
        {
            birdListView.UpdateBirdList(birdService.GetAllBirds());
        }
    }
}
