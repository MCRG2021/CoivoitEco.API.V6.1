﻿using System.Data;
using CoiviteEco.APP.Service.Annonce.Queries;
using CovoitEco.APP.Model.Models;
using CovoitEco.APP.Service.Annonce.Commands;
using CovoitEco.APP.Service.Facture.Commands;
using CovoitEco.APP.Service.Facture.Queries;
using CovoitEco.APP.Service.Reservation.Commands;
using CovoitEco.APP.Service.Reservation.Queries;
using CovoitEco.APP.Service.Utilisateur.Queries;
using CovoitEco.APP.Service.Vehicule.Commands;
using CovoitEco.APP.Service.Vehicule.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace CovoitEco.APP.Components
{
    public class BaseComponent : ComponentBase
    {
        #region Parameter

        [Parameter]
        public static int idVehicule { get; set; }

        [Parameter]
        public static int idAnnonce { get; set; }

        [Parameter]
        public static int idReservation { get; set; }

        [Parameter]
        public static int idFacture { get; set; }

        [Parameter]
        public static int idUser { get; set; } = 1; // idUser must be edit (default 1 for the moment => see data)

        [Parameter]
        public static bool confirme { get; set; }

        [Parameter]
        public static string AccessToken { get; set; }

        #region Response

        [Parameter]
        public ReservationUserProfileVm responseGetAllReservationUser { get; set; }

        [Parameter]
        public VehiculeProfileVm responseGetVehicule { get; set; }

        [Parameter]
        public AnnonceProfileVm responseAnnonce { get; set; }

        [Parameter]
        public VehiculeProfileVm responseGetAllVehicule { get; set; } 

        [Parameter]
        public ReservationUserProfileVm responseGetReservationUser { get; set; } 

        [Parameter]
        public UserProfileVm responseGetUtilisateurProfile { get; set; } 

        [Parameter]
        public ReservationProfileVm responseReservationProfile { get; set; }

        #endregion

        #endregion

        #region Formular

        public ReservationFormular requestReservationFormular { get; set; } = new ReservationFormular(); 

        public AnnonceProfileFormular requestAnnonceProfileFormular { get; set; } = new AnnonceProfileFormular(); 

        public static AnnonceRechercheFormular requestAnnonceRechercheFormular { get; set; } = new AnnonceRechercheFormular();  // static 

        public VehiculeProfileFormular resquestVehiculeProfileFormular { get; set; } = new VehiculeProfileFormular() { VEH_UTL_Id = idUser }; 

        #endregion

        #region Inject

        [Inject]
        public IVehiculeQueriesService vehiculeQueries { get; set; }

        [Inject]
        public IVehiculeCommandsService vehiculeCommands { get; set; } 

        [Inject]
        public IAnnonceQueriesService AnnonceQueries { get; set; } 

        [Inject]
        public IAnnonceCommandsService AnnonceCommands { get; set; } 

        [Inject]
        public IReservationCommandsService ReservationCommands { get; set; } 

        [Inject]
        public IReservationQueriesService ReservationQueries { get; set; } 

        [Inject]
        public IFactureCommandsService FactureCommands { get; set; } 

        [Inject]
        public IFactureQueriesService FactureQueries { get; set; } 

        [Inject]
        public IUtilisateurQueriesService UtilisateurQueries { get; set; }

        [Inject]
        IAccessTokenProvider TokenProvider { get; set; } // test

        #endregion


        public void UpdateIdAnnonce(int id)
        {
            idAnnonce = id;
        }

        public void UpdateIdReservation(int id)
        {
            idReservation = id;
        }

        public void UpdateIFacutre(int id)
        {
            idFacture = id;
        }

        protected async Task SetIdVehCurrent()
        {
            responseGetVehicule = await vehiculeQueries.GetVehiculeProfile(idUser); 
            if (responseGetVehicule.Lists.Count == 0)
            {
                throw new Exception(); // to edit (message you have no vehicule profile)
            }
            else
            {
                idVehicule = responseGetVehicule.Lists[0].VEHPR_Id;
            }
        }

        protected async Task SetIdUser()
        {
            // to edit
        }

        // use to tested a token access 
        protected async Task GetAccessToken()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            AccessToken = string.Empty;

            if (accessTokenResult.TryGetToken(out var token))
            {
                AccessToken = token.Value;
            }
        }

    }
}
