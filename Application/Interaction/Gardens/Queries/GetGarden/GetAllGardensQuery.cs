﻿using Application.Common.Interfaces;
using Application.Interaction.Gardens.Queries.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Interaction.Gardens.Queries.GetGarden
{
    public class GetAllGardensQuery : IRequest<GardensVm>
    {

    }

    public class GetAllGardensQueryHandler : IRequestHandler<GetAllGardensQuery, GardensVm>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllGardensQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GardensVm> Handle(GetAllGardensQuery request, CancellationToken cancellationToken) 
        {
            GardensVm garden =  new GardensVm
            {
                GardenList = await _context.Gardens
                .ProjectTo<GardenDTO>(_mapper.ConfigurationProvider)
                    .Where(g => !g.IsDeleted && !g.Owner.IsDeleted)
                    .OrderBy(g => g.Id)
                    .ToListAsync(cancellationToken)
            };
            Console.WriteLine(garden);
            return garden;
        } 
    }
}
