using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _db;

        public PositionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActivePosition(Position position)
        {
            _db.Positions.Update(position);
            return Save();
        }

        public bool CreatePosition(Position position)
        {
            _db.Positions.Add(position);
            return Save();
        }

        public bool DeletePosition(Position position)
        {
            _db.Positions.Remove(position);
            return Save();
        }

        public bool PositionExists(string name)
        {
            bool value = _db.Positions.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool PositionExists(int id)
        {
            return _db.Positions.Any(a => a.Id == id);
        }

        public Position GetPosition(int positionId)
        {
            return _db.Positions.Include(c => c.Department).FirstOrDefault(a => a.Id == positionId);
        }

        public ICollection<Position> GetPositions()
        {
            return _db.Positions.OrderBy(a => a.Name).ToList();
        }

        public bool InactivePosition(Position position)
        {
            _db.Positions.Update(position);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdatePosition(Position position)
        {
            _db.Positions.Update(position);
            return Save();
        }

        public ICollection<Position> GetPositionsInDepartment(int departmentId)
        {
            return _db.Positions.Include(c => c.Department)
                .Where(c => c.DepartmentId == departmentId).ToList();

        }
    }
}
