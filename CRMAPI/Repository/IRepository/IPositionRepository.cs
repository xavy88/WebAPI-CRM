using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IPositionRepository
    {
        ICollection<Position> GetPositions();
        ICollection<Position> GetPositionsInDepartment(int departmentId);
        Position GetPosition(int positionId);
        bool PositionExists(string name);
        bool PositionExists(int id);
        bool CreatePosition(Position position);
        bool UpdatePosition(Position position);
        bool InactivePosition(Position position);
        bool ActivePosition(Position position);
        bool DeletePosition(Position position);
        bool Save();
    }
}

