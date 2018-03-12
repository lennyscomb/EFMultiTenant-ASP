using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using EFMultiTenant.Models;

namespace MultiTenantEF_ASP.Models
{
    public static class TransactionManager
    {
        //private static IEntityCreationListener _entityCreationListener = new NullEntityCreationListener();
        //private static IEntityDeletionListener _entityDeletionListener = new NullEntityDeletionListener();
        //private static IUowCreationStrategy _uowCreationStrategy = new DefaultUnitOfWorkCreationStrategy();

        public static dynamic Execute(string functionName, Func<object> functionToExecute)
        {
            try
            {
                //if (Profiler.Profiler.IS_ON) Profiler.Profiler.Instance.StartRecording(functionName);
                using (var uow = CreateUow())
                {
                    using (var dbContextTransaction = uow.GetDatabase().BeginTransaction())
                    {
                        try
                        {
                            SetUowOnThread(uow);
                            var retVal = functionToExecute.Invoke();
                            uow.SaveChanges();
                            dbContextTransaction.Commit();
                            return retVal;
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            Console.WriteLine(e.StackTrace);
                            throw e;
                        }
                    }
                }
            }
            catch (DbEntityValidationException dbve)
            {
                foreach (var dbEntityValidationResult in dbve.EntityValidationErrors)
                {
                    foreach (var validationError in dbEntityValidationResult.ValidationErrors)
                    {
                        Console.WriteLine("Validation error '{0}' for property '{1}'", validationError.ErrorMessage, validationError.PropertyName);
                    }
                }
                throw dbve;
            }
            finally
            {
                ClearUowOnThread();
            }

        }

        public static dynamic Execute(Func<object> functionToExecute)
        {
            try
            {
                using (var uow = CreateUow())
                {
                    SetUowOnThread(uow);
                    var retVal = functionToExecute.Invoke();
                    uow.SaveChanges();
                    return retVal;
                }
            }

            finally
            {
                ClearUowOnThread();
            }
        }

        private static IUnitOfWork CreateUow()
        {
            return new EFMultiTenantDbContext();
        }

        private static System.Object unitOfWorkClearLock = new System.Object();
        private static void ClearUowOnThread()
        {
            lock (unitOfWorkClearLock)
            {
                System.Threading.Thread.FreeNamedDataSlot("" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        private static System.Object unitOfWorkSetLock = new System.Object();
        private static void SetUowOnThread(IUnitOfWork uow)
        {
            lock (unitOfWorkSetLock)
            {
                LocalDataStoreSlot lds =
                    System.Threading.Thread.GetNamedDataSlot("" +
                                                                  System.Threading.Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.SetData(lds, uow);
            }
        }

        private static System.Object unitOfWorkGetLock = new System.Object();
        public static IUnitOfWork UnitOfWork()
        {
            lock (unitOfWorkGetLock)
            {
                LocalDataStoreSlot lds = System.Threading.Thread.GetNamedDataSlot("" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                var unitOfWork = (IUnitOfWork)System.Threading.Thread.GetData(lds);
                if (unitOfWork == null)
                {
                    throw new Exception("A unit of work has not been started for the current thread.");
                }
                return unitOfWork;

            }
        }


        public static bool HasActiveUnitOfWork()
        {
            lock (unitOfWorkGetLock)
            {
                LocalDataStoreSlot lds = System.Threading.Thread.GetNamedDataSlot("" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                var unitOfWork = (IUnitOfWork)System.Threading.Thread.GetData(lds);
                return unitOfWork != null;
            }
        }

        //public static void SetEntityCreationListner(IEntityCreationListener creationListner)
        //{
        //    _entityCreationListener = creationListner;
        //}

        //public static void SetEntityDeletionListner(IEntityDeletionListener deletionListner)
        //{
        //    _entityDeletionListener = deletionListner;
        //}

        //public static void NotifyEntityCreated(IEntity entity)
        //{
        //    _entityCreationListener.NotifyCreated(entity);
        //}

        //public static void NotifyEntityDeleted(IEntity entity)
        //{
        //    _entityDeletionListener.NotifyRemoved(entity);
        //}

    }
}