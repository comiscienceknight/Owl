using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Owl.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageTonightImGoingToNeighborhood : Page
    {
        public PageTonightImGoingToNeighborhood()
        {
            this.InitializeComponent();
            this.Loaded += PageTonightImGoingToNeighborhood_Loaded;
            this.DataContext = new NeighborhoodViewModel();
        }

        void PageTonightImGoingToNeighborhood_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AppBarButton_Leave_Click(object sender, RoutedEventArgs e)
        {
            App.QuitFromEditPost();
        }

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void AppBarButton_Forward_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageImWithGirlsAndGuys));
        }

        private void Button_SearchByMetro_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_Stations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListView_Stations_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            if(listView != null)
            {
                var checkedItem = e.ClickedItem as ParisSubwayStation;
                if (checkedItem != null)
                {
                    App.MyPreviewPost.OutType = "Neighborhood - " + checkedItem.SubwayStationName;
                    App.MyPreviewPost.VenueId = null;
                    App.MyPreviewPost.Place = null;
                    App.MyPreviewPost.PlaceAddresse = null;
                    var rootFrame = (Window.Current.Content as Frame);
                    rootFrame.Navigate(typeof(PageImWithGirlsAndGuys));
                }
            }
        }
    }


    public class ParisSubwayStation
    {
        public string SubwayStationName { get; set; }
        public string Arrondissement { get; set; }
    }

    public class NeighborhoodViewModel
    {
        private IList data;

        public IList Data
        {
            get
            {
                if (data == null)
                {
                    var items = StationsDictionary.CreateSampleData();
                    data = items.ToAlphaGroups(x => x.SubwayStationName);
                }
                return data;
            }
        }
        private CollectionViewSource collection;

        public CollectionViewSource Collection
        {
            get
            {
                if (collection == null)
                {
                    collection = new CollectionViewSource();
                    collection.Source = Data;
                    collection.IsSourceGrouped = true;
                }
                return collection;
            }
        }
    }

    public class StationsDictionary
    {
        private static Dictionary<string, string> _stationsDic = new Dictionary<string, string>()
        {
            {"Abbesses", "75018"}, {"Alésia", "75014"}, {"Alexandre Dumas", "75011-75020"},
            {"Alma – Marceau", "75016"}, {"Anatole France", "Levallois-Perret"}, {"Anvers", "75009-75018"},
            {"Argentine", "75016-75017"}, {"Arts et Métiers", "75003"}, {"Asnières – Gennevilliers – Les Courtilles", "Asnières-sur-Seine, Gennevilliers"},
            {"Assemblée Nationale", "75007"}, {"Aubervilliers – Pantin – Quatre Chemins", "Aubervilliers"}, {"Avenue Émile Zola", "75015"},
            {"Avron", "75011-75020"}, {"Bagneux", "Bagneux"}, {"Balard", "75015"},
            {"Barbès – Rochechouart", "75009,10,18"}, {"Basilique de Saint-Denis", "Saint-Denis"}, {"Bastille", "75004,11,12"},
            {"Bel-Air", "75012"}, {"Belleville", "75010,11,19,20"}, {"Bérault", "Saint-Mandé, Vincennes"},
            {"Bercy", "75012"}, {"Bibliothèque François Mitterrand", "75013"}, {"Billancourt", "Boulogne-Billancourt"},
            {"Bir-Hakeim", "75015"}, {"Blanche", "75009,18"}, {"Bobigny – Pablo Picasso", "Bobigny"},
            {"Boissière", "75016"}, {"Bolivar", "75019"}, {"Bonne Nouvelle", "75002,9,10"},
            {"Botzaris", "75019"}, {"Boulogne – Jean Jaurès", "Boulogne-Billancourt"}, {"Boulogne – Pont de Saint-Cloud", "Boulogne-Billancourt"},
            {"Boucicaut", "75015"}, {"Bourse", "75002"}, {"Bréguet – Sabin", "75011"},
            {"Brochant", "75017"}, {"Buttes Chaumont", ""}, {"Buzenval", ""},
            {"Cadet", ""}, {"Cambronne", ""}, {"Campo Formio", ""},
            {"Cardinal Lemoine", ""}, {"Carrefour Pleyel", ""}, {"Censier – Daubenton", ""},
            {"Champs-Élysées – Clemenceau", ""}, {"Chardon Lagache", ""}, {"Charenton – Écoles", ""},
            {"Charles de Gaulle – Étoile", ""}, {"Charles Michels", ""}, {"Charonne", ""},
            {"Château d'Eau", ""}, {"Château de Vincennes", ""}, {"Château-Landon", ""},
            {"Château Rouge", ""}, {"Châtelet", ""}, {"Châtillon – Montrouge", ""},
            {"Chaussée d'Antin – La Fayette", ""}, {"Chemin Vert", ""}, {"Chevaleret", ""},
            {"Cité", ""}, {"Cluny – La Sorbonne", ""}, {"Colonel Fabien", ""},
            {"Commerce", ""}, {"Concorde", ""}, {"Convention", ""},
            {"Corentin Cariou", ""}, {"Corentin Celton", ""}, {"Corvisart", ""},
            {"Cour Saint-Émilion", ""}, {"Courcelles", ""}, {"Couronnes", ""},  
            {"Créteil – L'Échat", ""}, {"Créteil – Préfecture", ""}, {"Créteil – Université", ""},     
            {"Crimée", ""}, {"Croix de Chavaux", ""}, {"Danube", ""},   
            {"Daumesnil", ""}, {"Denfert-Rochereau", ""}, {"Dugommier", ""},   
            {"Dupleix", ""}, {"Duroc", ""}, {"École Militaire", ""},   
            {"École Vétérinaire de Maisons-Alfort	", ""}, {"Edgar Quinet", ""}, {"Église d'Auteuil", ""},   
            {"Église de Pantin", ""}, {"Esplanade de La Défense", ""}, {"Étienne Marcel", ""},   
            {"Europe", ""}, {"Exelmans", ""}, {"Faidherbe – Chaligny", ""},   
            {"Falguière", ""}, {"Félix Faure", ""}, {"Filles du Calvaire", ""},   
            {"Fort d'Aubervilliers", ""}, {"Franklin D. Roosevelt", ""}, {"Gabriel Péri", ""},   
            {"Gaîté", ""}, {"Gallieni", ""}, {"Gambetta", ""},   
            {"Gare d'Austerlitz", ""}, {"Gare de l'Est", ""}, {"Gare de Lyon", ""},   
            {"Gare du Nord", ""}, {"George V", ""}, {"Glacière", ""},   
            {"Goncourt", ""}, {"Grands Boulevards", ""}, {"Guy Môquet", ""},   
            {"Havre – Caumartin", ""}, {"Hoche", ""}, {"Hôtel de Ville", ""},   
            {"Invalides", ""}, {"Jacques Bonsergent", ""}, {"Jasmin", ""},   
            {"Jaurès", ""}, {"Javel – André Citroën", ""}, {"Jourdain", ""},   
            {"Jules Joffrin", ""}, {"Jussieu", ""}, {"Kléber", ""},   
            {"La Chapelle", ""}, {"La Courneuve – 8 Mai 1945", ""}, {"La Défense – Grande Arche", ""},   
            {"La Fourche", ""}, {"La Motte-Picquet – Grenelle", ""}, {"La Muette", ""},  
            {"La Tour-Maubourg", ""}, {"Lamarck – Caulaincourt", ""}, {"Laumière", ""},  
            {"Le Kremlin-Bicêtre", ""}, {"Le Peletier", ""}, {"Ledru-Rollin", ""},   
            {"Les Agnettes", ""}, {"Les Gobelins", ""}, {"Les Halles", ""},  
            {"Les Sablons", ""}, {"Liberté", ""}, {"Liège", ""},   
            {"Louis Blanc", ""}, {"Louise Michel", ""}, {"Lourmel", ""},  
            {"Louvre – Rivoli", ""}, {"Mabillon", ""}, {"Madeleine", ""},   
            {"Mairie d'Issy", ""}, {"Mairie d'Ivry", ""}, {"Mairie de Clichy", ""},  
            {"Mairie de Montreuil", ""}, {"Mairie de Montrouge", ""}, {"Mairie de Saint-Ouen", ""},   
            {"Mairie des Lilas", ""}, {"Maison Blanche", ""}, {"Maisons-Alfort – Les Juilliottes", ""},  
            {"Maisons-Alfort – Stade", ""}, {"Malakoff – Plateau de Vanves", ""}, {"Malakoff – Rue Étienne Dolet", ""},   
            {"Malesherbes", ""}, {"Maraîchers", ""}, {"Marcadet – Poissonniers", ""},  
            {"Marcel Sembat", ""}, {"Marx Dormoy", ""}, {"Maubert-Mutualité", ""},   
            {"Ménilmontant", ""}, {"Michel Bizot", ""}, {"Michel-Ange – Auteuil", ""},  
            {"Michel-Ange – Molitor", ""}, {"Mirabeau", ""}, {"Miromesnil", ""},   
            {"Monceau", ""}, {"Montgallet", ""}, {"Montparnasse – Bienvenüe", ""},  
            {"Mouton-Duvernet", ""}, {"Nation", ""}, {"Nationale", ""},   
            {"Notre-Dame-de-Lorette", ""}, {"Notre-Dame-des-Champs", ""}, {"Oberkampf", ""},  
            {"Odéon", ""}, {"Olympiades", ""}, {"Opéra", ""},   
            {"Ourcq", ""}, {"Palais Royal – Musée du Louvre", ""}, {"Parmentier", ""},  
            {"Passy", ""}, {"Pasteur", ""}, {"Pelleport", ""},   
            {"Père Lachaise", ""}, {"Pereire", ""}, {"Pernety", ""},  
            {"Philippe Auguste", ""}, {"Picpus", ""}, {"Pierre et Marie Curie", ""},   
            {"Pigalle", ""}, {"Place d'Italie", ""}, {"Place de Clichy", ""},  
            {"Place des Fêtes", ""}, {"Place Monge", ""}, {"Plaisance", ""},   
            {"Pointe du Lac", ""}, {"Poissonnière", ""}, {"Pont de Levallois – Bécon", ""},  
            {"Pont de Neuilly", ""}, {"Pont de Sèvres", ""}, {"Pont Marie", ""},  
            {"Pont Neuf", ""}, {"Porte d'Auteuil", ""}, {"Porte d'Italie", ""},  
            {"Porte d'Ivry", ""}, {"Porte d'Orléans", ""}, {"Porte Dauphine", ""},  
            {"Porte de Bagnolet", ""}, {"Porte de Champerret", ""}, {"Porte de Charenton", ""},  
            {"Porte de Choisy", ""}, {"Porte de Clichy", ""}, {"Porte de Clignancourt", ""},  
            {"Porte de la Chapelle", ""}, {"Porte de la Villette", ""}, {"Porte de Montreuil", ""},  
            {"Porte de Pantin", ""}, {"Porte de Saint-Cloud", ""}, {"Porte de Saint-Ouen", ""},  
            {"Porte de Vanves", ""}, {"Porte de Versailles", ""}, {"Porte de Vincennes", ""},  
            {"Porte des Lilas", ""}, {"Porte Dorée", ""}, {"Porte Maillot", ""},  
            {"Pré Saint-Gervais", ""}, {"Pyramides", ""}, {"Pyrénées", ""},  
            {"Quai de la Gare", ""}, {"Quai de la Rapée", ""}, {"Quatre-Septembre", ""},  
            {"Rambuteau", ""}, {"Ranelagh", ""}, {"Raspail", ""},  
            {"Réaumur – Sébastopol", ""}, {"Rennes", ""}, {"République", ""},  
            {"Reuilly – Diderot", ""}, {"Richard-Lenoir", ""}, {"Richelieu – Drouot", ""},  
            {"Riquet", ""}, {"Robespierre", ""}, {"Rome", ""},  
            {"Rue de la Pompe", ""}, {"Rue des Boulets", ""}, {"Rue du Bac", ""},  
            {"Rue Saint-Maur", ""}, {"Saint-Ambroise", ""}, {"Saint-Augustin", ""},  
            {"Saint-Denis – Porte de Paris", ""}, {"Saint-Denis – Université", ""}, {"Saint-Fargeau", ""},  
            {"Saint-François-Xavier", ""}, {"Saint-Germain-des-Prés", ""}, {"Saint-Georges", ""},  
            {"Saint-Jacques", ""}, {"Saint-Lazare", ""}, {"Saint-Mandé", ""},  
            {"Saint-Marcel", ""}, {"Saint-Michel", ""}, {"Saint-Paul", ""},    
            {"Saint-Philippe du Roule", ""}, {"Saint-Placide", ""}, {"Saint-Sébastien – Froissart", ""},    
            {"Saint-Sulpice", ""}, {"Ségur", ""}, {"Sentier", ""},    
            {"Sèvres – Babylone", ""}, {"Sèvres – Lecourbe", ""}, {"Simplon", ""},    
            {"Solférino", ""}, {"Stalingrad", ""}, {"Strasbourg – Saint-Denis", ""},    
            {"Sully – Morland", ""}, {"Télégraphe", ""}, {"Trinité – d'Estienne d'Orves", ""},    
            {"Ternes", ""}, {"Tolbiac", ""}, {"xxxxxx", ""},    
            {"Trocadéro", ""}, {"Tuileries", ""}, {"Vaneau", ""},    
            {"Varenne", ""}, {"Vaugirard", ""}, {"Vavin", ""},    
            {"Verdun-Sud", ""}, {"Victor Hugo", ""}, {"Villejuif – Léo Lagrange", ""},    
            {"Villejuif – Louis Aragon", ""}, {"Villejuif – Paul Vaillant-Couturier", ""}, {"Villiers", ""},     
            {"Volontaires", ""}, {"Voltaire", ""}, {"Wagram", ""}
        };

        public static List<ParisSubwayStation> CreateSampleData()
        {
            
            List<ParisSubwayStation> stations = new List<ParisSubwayStation>();
            foreach (var station in _stationsDic)
            {
                stations.Add(new ParisSubwayStation()
                {
                    SubwayStationName = station.Key,
                    Arrondissement = station.Value
                });
            }
            return stations;
        }
    }
}
