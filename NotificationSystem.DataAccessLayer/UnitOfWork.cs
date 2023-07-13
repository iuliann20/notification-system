using Microsoft.Extensions.Options;
using NotificationSystem.Common.Interfaces;
using NotificationSystem.Common.Settings;
using NotificationSystem.DataAccessLayer.Implementation;
using NotificationSystem.DataAccessLayer.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace NotificationSystem.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IAttachmentRepository _attachmentRepository;
        private IEmailRepository _emailRepository;
        private ISourceRepository _sourceRepository;

        private bool _disposed;
        private readonly AppSettings _appSettings;
        public UnitOfWork(IOptions<AppSettings> appSettings, IEncryptionService encryptionService)
        {
            _appSettings = appSettings.Value;

            _connection = new SqlConnection(encryptionService.Decrypt(_appSettings.DatabaseConnection));

            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IEmailRepository EmailRepository => _emailRepository ?? (_emailRepository = new EmailRepository(_transaction, _connection));
        public IAttachmentRepository AttachmentRepository => _attachmentRepository ?? (_attachmentRepository = new AttachmentRepository(_transaction, _connection));
        public ISourceRepository SourceRepository => _sourceRepository ?? (_sourceRepository = new SourceRepository(_transaction, _connection));



        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }

        private void ResetRepositories()
        {
            _emailRepository = null;
            _attachmentRepository = null;
            _sourceRepository = null;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
