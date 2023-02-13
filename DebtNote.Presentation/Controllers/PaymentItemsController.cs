﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/payments/{paymentId}/payment-items")]
    [ApiController]
    public class PaymentItemsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public PaymentItemsController(IServiceManager service) => _service = service;

        [HttpGet]
        public IActionResult GetPaymentItems(Guid paymentId, Guid skuId)
        {
            var paymentItems = _service.PaymentItemService.GetAllPaymentItems(paymentId, skuId, trackChanges: false);
            return Ok(paymentItems);
        }

        [HttpGet("{id:guid}",Name = "GetItemForPayment")]
        public IActionResult GetPaymentItem(Guid paymentId, Guid skuId, Guid id)
        {
            var paymentItem = _service.PaymentItemService.GetPaymentItem(paymentId, skuId, id, trackChanges: false);
            return Ok(paymentItem);
        }

        [HttpPost]
        public IActionResult CreateItemForPayment
            (Guid paymentId, [FromBody] PaymentItemForCreationDTO paymentItem)
        {
            if (paymentItem is null)
                return BadRequest("PaymentItemForCreationDto object is null");
            var ItemToReturn =
            _service.PaymentItemService.CreatePaymentItem(paymentId, paymentItem, trackChanges: false);
            return CreatedAtRoute("GetItemForPayment", new 
            { paymentId, ItemToReturn.SkuId, id = ItemToReturn.Id }, ItemToReturn);
        }

    }
}
