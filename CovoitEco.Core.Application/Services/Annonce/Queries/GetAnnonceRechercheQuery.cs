﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CovoitEco.Core.Application.Common.Interfaces;
using CovoitEco.Core.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CovoitEco.Core.Application.Services.Annonce.Queries
{
    public class GetAnnonceRechercheQuery : IRequest<AnnonceProfileVm>
    {
        public DateTime ANNREC_DateDepart { get; set; }
        public string ANNREC_VilleDepart { get ; set; }
        public string ANNREC_VilleArrive { get; set; }
    }
    public class GetAnnonceRechercheQueryHandler : IRequestHandler<GetAnnonceRechercheQuery, AnnonceProfileVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;


        public GetAnnonceRechercheQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AnnonceProfileVm> Handle(GetAnnonceRechercheQuery request, CancellationToken cancellationToken) // no tested again 
        {
            List<AnnonceProfileDTO> listAnnonce = await (
                from a in _context.Annonce
                join s in _context.StatutAnnonce on a.ANN_STATANN_Id equals s.STATANN_Id
                join u in _context.Utilisateur on a.ANN_UTL_Id equals u.UTL_Id
                join v in _context.Vehicule on a.ANN_VEH_Id equals v.VEH_Id
                where (a.ANN_DateDepart >= request.ANNREC_DateDepart)
                select new AnnonceProfileDTO()
                {
                    ANNPR_Id = a.ANN_Id,
                    ANNPR_DatePublication = a.ANN_DatePublication,
                    ANNPR_Prix = a.ANN_Prix,
                    ANNPR_LocaliteDepart = a.ANN_LocaliteDepart,
                    ANNPR_LocaliteArrive = a.ANN_LocaliteArrive,
                    ANNPR_DateDepart = a.ANN_DateDepart,
                    ANNPR_DateArrive = a.ANN_DateArrive,
                    ANNPR_Statut = s.STATANN_Libelle,
                    ANN_OptAutoroute = a.ANN_OptAutoroute,
                    ANN_OptFumeur = a.ANN_OptFumeur,
                    ANN_OptAnimaux = a.ANN_OptAnimaux,
                    ANNPR_AnnonceurNom = u.UTL_Nom,
                    ANNPR_AnnonceurPrenom = u.UTL_Prenom,
                    ANNPR_VehImmatriculation = v.VEH_Immatriculation
                }
            ).ToListAsync(cancellationToken);

            List<AnnonceProfileDTO> sortedListAnnonce = new List<AnnonceProfileDTO>();

            foreach (var a in listAnnonce)
            {
                string arrCity = ExtractCity(a.ANNPR_LocaliteArrive);
                if ((ExtractCity(a.ANNPR_LocaliteDepart) == UpperCaseFirstLetter(request.ANNREC_VilleDepart)) &&
                    (ExtractCity(a.ANNPR_LocaliteArrive) == UpperCaseFirstLetter(request.ANNREC_VilleArrive)) &&
                    (TooLate(a.ANNPR_DateDepart) == false)) 
                {
                    sortedListAnnonce.Add(a);
                }
            }

            return new AnnonceProfileVm()
            {
              Lists = sortedListAnnonce
            };

        }

        private string ExtractCity(string address) 
        {
            char[] spearator = {' ',','};

            String[] city = address.Split(spearator);

            return city[4];
        }

        private bool ConfirmFirstLetterUpper(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;

            return char.IsUpper(word[0]) ? true : false;
        }

        private string UpperCaseFirstLetter(string word)
        {
            if(!ConfirmFirstLetterUpper(word)) return char.ToUpper(word[0]) + word.Substring(1);
            return word;
        }

        private bool TooLate(DateTime dateTimeDepart)
        {
            double minutes = 0;
            if (dateTimeDepart.Date >= DateTime.Now.Date)
            {
                TimeSpan timespan = dateTimeDepart - DateTime.Now;
                minutes = timespan.TotalMinutes;
                if (minutes >= 15) return false;
            }
            return true;
        }
    }
}
