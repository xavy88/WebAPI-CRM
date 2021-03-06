using AutoMapper;
using CRMAPI.Models;
using CRMAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.CRMMapper
{
    public class CRMMappings : Profile
    {
        public CRMMappings()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Position, PositionUpdateDto>().ReverseMap();
            CreateMap<Position, PositionCreateDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeeUpdateDto>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Contact, ContactUpdateDto>().ReverseMap();
            CreateMap<Contact, ContactCreateDto>().ReverseMap();
            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<Service, ServiceUpdateDto>().ReverseMap();
            CreateMap<Service, ServiceCreateDto>().ReverseMap();
            CreateMap<Models.Task, TaskCreateDto>().ReverseMap();
            CreateMap<Models.Task, TaskDto>().ReverseMap();
            CreateMap<Models.Task, TaskUpdateDto>().ReverseMap();
            CreateMap<TaskAssignment, TaskAssignmentDto>().ReverseMap();
            CreateMap<TaskAssignment, TaskAssignmentUpsertDto>().ReverseMap();
        }
    }
}
