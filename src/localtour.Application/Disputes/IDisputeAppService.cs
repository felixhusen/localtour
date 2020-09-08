﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Disputes.Dto;
using System.Threading.Tasks;

namespace localtour.Disputes
{
    public interface IDisputeAppService : IApplicationService
    {
        Task<PagedResultDto<GetDisputeForViewDto>> GetAll(GetAllDisputesInput input);

        Task<GetDisputeForViewDto> GetDisputeForView(int id);

        Task<GetDisputeForEditOutput> GetDisputeForEdit(EntityDto input);

        Task<FileDto> GetDisputesToExcel(GetAllDisputesInput input);

        Task CreateOrEdit(CreateOrEditDisputeDto input);

        Task Delete(EntityDto input);
    }
}
