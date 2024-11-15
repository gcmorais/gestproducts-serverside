﻿using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Deactivate;
using dotnet8_user.Application.UseCases.ProductUseCases.Delete;
using dotnet8_user.Application.UseCases.ProductUseCases.GetAll;
using dotnet8_user.Application.UseCases.ProductUseCases.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet8_user.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProductResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ProductGetAllRequest(), cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> Update(Guid id, ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id) return BadRequest();

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("Deactivate")]
        public async Task<IActionResult> DeactivateProduct(Guid productId)
        {
            var response = await _mediator.Send(new ProductDeactivateRequest(productId));
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id is null) return BadRequest();

            var productDeleteRequest = new ProductDeleteRequest(id.Value);

            var response = await _mediator.Send(productDeleteRequest, cancellationToken);
            return Ok(response);
        }
    }
}
