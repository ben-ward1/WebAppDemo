﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DTOs;
using Demo.GeneralFunctions;

namespace Demo.DAL
{
    /// <summary>
    /// Hardcoded repository meant to mimic data access to a Db context for demonstration puroposes
    /// </summary>
    public class FakeRepository : IDisposable
    {
        private List<ItemModelDTO> FakeContext;

        /// <summary>
        /// Contructor automatically populates FakeContext.
        /// </summary>
        /// <param name="recordCount">Optional parameter. Intended to allow the user
        /// to test performance by varying page size.</param>
        public FakeRepository(int recordCount = 20)
        {
            FakeContext = CreateRecordsForContextList(recordCount);
        }

        /// <summary>
        /// Selects a name at random from a bank.
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private string PickANameForSingleRecord(Random rnd)
        {
            var ng = new NameGenerator();
            return ng.PickAName(rnd);
        }

        /// <summary>
        /// Creates a list of records.
        /// </summary>
        /// <param name="recordCount">Sets the number of records to be created for FakeContext</param>
        /// <returns></returns>
        private List<ItemModelDTO> CreateRecordsForContextList(int recordCount)
        {
            var recordList = new List<ItemModelDTO>();
            var rnd = new Random();

            //Populates record with ItemModelDTOs that vary on id, name, and organization.
            for (int i = 0; i < recordCount / 3; ++i)
            {
                recordList.Add(CreateSingleRecord(3*i, "ThisDescription", "Sales", rnd));
                recordList.Add(CreateSingleRecord(3*i + 1, "ThatDescription", "Accounting", rnd));
                recordList.Add(CreateSingleRecord(3*i + 2, "OtherDescription", "Marketing", rnd));
            }

            //The following blocks resolve cases where recordCount is not divisible by three, meaning
            //the block above has not completely finished creating the last one or two records.
            if (recordCount % recordList.Count() != 0)
            {
                recordList.Add(CreateSingleRecord(recordCount - (recordCount % recordList.Count()),"ThisDescription", "Sales", rnd));
            }
            if (recordCount % recordList.Count() == 1)
            {
                recordList.Add(CreateSingleRecord(recordCount - 1, "ThatDescription", "Sales", rnd));
            }

            return recordList;
        }

        /// <summary>
        /// Creates a single ItemModelDTO
        /// </summary>
        /// <param name="counter">Used to set ItemId</param>
        /// <param name="description"></param>
        /// <param name="org">organization</param>
        /// <param name="basePrice">Used to avoid Price == 0</param>
        /// <param name="rnd">Allows random variation between records.</param>
        /// <returns></returns>
        private ItemModelDTO CreateSingleRecord(int counter, string description, string org, Random rnd)
        {
            var record = new ItemModelDTO
            {
                ItemId = counter + 1,
                ItemName = description + counter.ToString(),
                Price = 1000.00M * rnd.Next(5, 21),
                Organization = org,
                POCName = PickANameForSingleRecord(rnd),
                DateEstablished = DateTime.Now.AddMonths(-rnd.Next(6, 25)),
                DateBegin = DateTime.Now.AddMonths(rnd.Next(-6, 13))
            };

            return record;
        }

        /// <summary>
        /// Gets a list of records filtered by organization.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public List<ItemModelDTO> GetFilteredItems(string organization)
        {
            var filteredItems = FakeContext.Where(x => x.Organization == organization)
                                           .OrderBy(x => x.ItemId)
                                           .ToList();

            return filteredItems;
        }

        /// <summary>
        /// Returns list of ItemModelDTO's
        /// </summary>
        public List<ItemModelDTO> GetItems()
        {
            //TODO this method doesn't do anything other than return FakeContext. A parameter for a filter model
            //can be added if needed in the future.
            return FakeContext.OrderBy(x => x.ItemId).ToList();
        }

        #region IDisposable Support
        //Disposable support is added here so that repository can be used similar to how
        //a business-sized repository might.

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FakeRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
