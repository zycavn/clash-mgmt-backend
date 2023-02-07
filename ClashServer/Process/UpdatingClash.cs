using ClashServer.Contracts;
using ClashServer.Entities;
using System;

namespace ClashServer.Process
{
    public class UpdatingClash
    {
        private readonly IRepositoryWrapper _repository;
        private readonly Clash _oldClash;
        private readonly Clash _newClash;

        public UpdatingClash(IRepositoryWrapper repository, Clash oldClash, Clash newClash)
        {
            _repository = repository;
            _oldClash = oldClash;
            _newClash = newClash;
        }

        public void ImplementUpdate()
        {
            UpdateNewInfo();
            AddNewStatus();
            AddNewComment();
        }

        private void AddNewStatus()
        {
            if (_oldClash.Status != _newClash.Status) {
                var status = new Status {
                    NewStatus = _newClash.Status,
                    UserName = "Html update",
                    Time = DateTime.Now,
                    ClashId = _oldClash.Id
                };
                _repository.Status.Create(status);
            }
        }

        private void AddNewComment()
        {
            if (_newClash.Comments != null) {
                foreach (var comment in _newClash.Comments) {
                    comment.ClashId = _oldClash.Id;
                    comment.UserName = comment.UserName;
                    comment.Description = comment.Description;
                    comment.Time = DateTime.Now;
                    _repository.Comment.Create(comment);
                }
            }
        }

        private void UpdateNewInfo()
        {
            _oldClash.Status = _newClash.Status;
            _oldClash.AssignTo = _newClash.AssignTo;
            _oldClash.GridLocation = _newClash.GridLocation;
            _oldClash.Description = _newClash.Description;
            _oldClash.DateFound = _newClash.DateFound;
            _oldClash.ClashPoint = _newClash.ClashPoint;
            _oldClash.Distance = _newClash.Distance;
            _oldClash.ElementId1 = _newClash.ElementId1;
            _oldClash.ElementId2 = _newClash.ElementId2;
            _oldClash.Layer1 = _newClash.Layer1;
            _oldClash.Layer2 = _newClash.Layer2;
            _oldClash.ItemName1 = _newClash.ItemName1;
            _oldClash.ItemName2 = _newClash.ItemName2;
            _oldClash.ItemType1 = _newClash.ItemType1;
            _oldClash.ItemType2 = _newClash.ItemType2;
            _oldClash.ItemPath1 = _newClash.ItemPath1;
            _oldClash.ItemPath2 = _newClash.ItemPath2;
            _oldClash.ClashImagePath = _newClash.ClashImagePath;
            _repository.Clash.Update(_oldClash);
        }
    }
}