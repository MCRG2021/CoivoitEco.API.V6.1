﻿using CovoitEco.APP.Model.Models;

namespace CovoitEco.APP.Service.Utilisateur.Queries
{
    public interface IUtilisateurQueriesService
    {
        public Task<UserProfileVm> GetUtilisateurPofile(int idUser);
    }
}
