﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Reviews.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Reviews
{

    public class ReviewAppService : localtourAppServiceBase, IReviewAppService
    {
        private readonly IRepository<Review, int> _reviewRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ReviewAppService(IRepository<Review, int> reviewRepository, IWebHostEnvironment hostEnvironment)
        {
            _reviewRepository = reviewRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetReviewForViewDto>> GetAll(GetAllReviewsInput input)
        {
            var filteredReviews = _reviewRepository.GetAll()
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Description.Contains(input.Query));

            var reviews = from o in filteredReviews

                        select new GetReviewForViewDto()
                        {
                            Review = new ReviewDto
                            {
                                Id = o.Id,
                                DatePosted = o.DatePosted,
                                Description = o.Description,
                                Rating = o.Rating,
                                TourId = o.TourId,
                                UserId = o.UserId
                            }
                        };

            var pagedAndFilteredReviews = reviews
                .OrderBy(input.Sorting ?? "Review.Id asc")
                .PageBy(input);

            var totalCount = await reviews.CountAsync();

            return new PagedResultDto<GetReviewForViewDto>(
                totalCount,
                await pagedAndFilteredReviews.ToListAsync()
            );
        }

        public async Task<GetReviewForViewDto> GetReviewForView(int id)
        {
            var review = await _reviewRepository.GetAsync(id);

            var output = new GetReviewForViewDto { Review = ObjectMapper.Map<ReviewDto>(review) };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Review_Edit)]
        public async Task<GetReviewForEditOutput> GetReviewForEdit(EntityDto input)
        {
            var review = await _reviewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReviewForEditOutput { Review = ObjectMapper.Map<CreateOrEditReviewDto>(review) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReviewDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Review_Create)]
        protected virtual async Task Create(CreateOrEditReviewDto input)
        {
            var review = ObjectMapper.Map<Review>(input);


            if (AbpSession.TenantId != null)
            {
                review.TenantId = (int?)AbpSession.TenantId;
            }


            await _reviewRepository.InsertAsync(review);
        }

        [AbpAuthorize(PermissionNames.Pages_Review_Edit)]
        protected virtual async Task Update(CreateOrEditReviewDto input)
        {
            var review = await _reviewRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, review);
        }

        [AbpAuthorize(PermissionNames.Pages_Review_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _reviewRepository.DeleteAsync(input.Id);
        }

    }
}
