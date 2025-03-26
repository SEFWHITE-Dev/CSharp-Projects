

namespace SurveillanceSystem
{
    class Program
    {
        static void Main(string[] args) { }
    }


    public class Employee : IEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
    }

    public interface IEmployee
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string JobTitle { get; set; }
    }


    public class EmployeeNotify : IObserver<ExternalVisitor>
    {
        public EmployeeNotify(IEmployee employee)
        {

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ExternalVisitor value)
        {
            throw new NotImplementedException();
        }
    }


    // remove the passed in observer obj from the passed in observers list
    public class Unsubscriber<ExternalVisitor> : IDisposable
    {
        private List<IObserver<ExternalVisitor>> _observers;
        private IObserver<ExternalVisitor> _observer;

        public Unsubscriber(List<IObserver<ExternalVisitor>> observers, IObserver<ExternalVisitor> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }


    public class SecuritySurveillanceHub : IObservable<ExternalVisitor>
    {
        private List<ExternalVisitor> _externalVisitors;
        private List<IObserver<ExternalVisitor>> _observers;

        public IDisposable Subscribe(IObserver<ExternalVisitor> observer)
        {
            if (!_observers.Contains(observer)) // check if the observer already exists
            {
                _observers.Add(observer); // if not a new observer is subscribing, add it to the list
            }

            foreach (var externalVisitor in _externalVisitors)
            {
                observer.OnNext(externalVisitor);
            }

            return new Unsubscriber<ExternalVisitor>(_observers, observer);
        }

        public void ConfirmExternalVisitorEntersBuilding(int id, string firstName, string lastName, string companyName, string jobTitle, DateTime entryDateTime, int employeeContactId)
        {
            ExternalVisitor externalVisitor = new ExternalVisitor
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                CompanyName = companyName,
                JobTitle = jobTitle,
                EntryDateTime = entryDateTime,
                InBuilding = true,
                EmployeeContactId = employeeContactId
            };

            _externalVisitors.Add(externalVisitor);

            foreach(var observer in _observers)
            {
                observer.OnNext(externalVisitor);
            }

        }

        public void ConfirmExternalVisitorExitsBuilding(int externalVisitorId, DateTime exitDateTime)
        {
            var externalVisitor = _externalVisitors.FirstOrDefault(e => e.Id == externalVisitorId);

            if(externalVisitor != null)
            {
                externalVisitor.ExitDateTime = exitDateTime;
                externalVisitor.InBuilding = false;

                foreach(var observer in _observers)
                {
                    observer.OnNext(externalVisitor);
                }
            }
        }

        public void BuildingEntryCutoffTimeReached()
        {
            // the cutoff time for new visitors has been reached, and all previous external visitors are outside the building
            if(_externalVisitors.Where(e => e.InBuilding == true).ToList().Count == 0)
            {
                foreach(var observer in _observers)
                {
                    observer.OnCompleted(); // no further notifications will be sent to the eventhandler
                }
            }
        }
    }



    public class ExternalVisitor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime ExitDateTime { get; set; }
        public bool InBuilding { get; set; }
        public int EmployeeContactId { get; set; }
    }

}