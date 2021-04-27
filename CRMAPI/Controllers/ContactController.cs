using AutoMapper;
using CRMAPI.Models;
using CRMAPI.Models.Dtos;
using CRMAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Controllers
{
    [Route("api/v{version:apiVersion}/contacts")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepo;
        private readonly IMapper _mapper;

        public ContactController(IContactRepository contactRepo, IMapper mapper)
        {
            _contactRepo = contactRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of contacts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<ContactDto>))]
        public IActionResult GetContacts()
        {
            var objList = _contactRepo.GetContacts();
            var objDto = new List<ContactDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ContactDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual contact.
        /// </summary>
        /// <param name="contactId">The Id of Contact</param>
        /// <returns></returns>

        [HttpGet("{contactId:int}", Name ="GetContact")]
        [ProducesResponseType(200, Type = typeof(ContactDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetContact(int contactId)
        {
            var obj = _contactRepo.GetContact(contactId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ContactDto>(obj);
            return Ok(objDto);

        }

        [HttpGet("[action]{accountId:int}")]
        [ProducesResponseType(200, Type = typeof(ContactDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetContactsInAccount(int accountId)
        {
            var objList = _contactRepo.GetContactsInAccount(accountId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<ContactDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ContactDto>(obj));
            }

           
            return Ok(objDto);

        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ContactDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateEmployee([FromBody] ContactCreateDto contactDto)
        {
            if (contactDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_contactRepo.ContactExists(contactDto.Name))
            {
                ModelState.AddModelError("", "Contact already exists!");
                return StatusCode(404, ModelState);
            }

            var contactObj = _mapper.Map<Contact>(contactDto);

            if (!_contactRepo.CreateContact(contactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {contactObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetContact", new { contactId=contactObj.Id},contactObj);
        }
        [HttpPatch("{contactId:int}", Name = "UpdateContact")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateContact(int contactId, [FromBody] ContactUpdateDto contactDto)
        {
            if (contactDto == null || contactId !=contactDto.Id )
            {
                return BadRequest(ModelState);
            }
            var contactObj = _mapper.Map<Contact>(contactDto);
           
            if (!_contactRepo.UpdateContact(contactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {contactObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{contactId:int}", Name = "DeleteContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteContact(int contactId)
        {
            if (!_contactRepo.ContactExists(contactId))
            {
                return NotFound();
            }
            var contactObj = _contactRepo.GetContact(contactId);

            if (!_contactRepo.DeleteContact(contactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {contactObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
