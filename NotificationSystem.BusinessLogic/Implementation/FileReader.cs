using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Common.Settings;
using NotificationSystem.DataAccessLayer;
using NotificationSystem.Models.Attachment;
using NotificationSystem.Models.Source;
using System.Net;

namespace NotificationSystem.BusinessLogic.Implementation
{
    public class FileReader : IFileReader
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public FileReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void WriteFile(Attachment attachment, int sourceID)
        {
            var sourceDto = _unitOfWork.SourceRepository.GetSourceById(sourceID).Result;
            var dirPath = GetSpecificPathForFile(attachment.FileName, sourceID);


            Directory.CreateDirectory(dirPath);

            var existingFilePath = Directory.GetFiles(dirPath, attachment.FileName, SearchOption.TopDirectoryOnly);
            var finalFileName = existingFilePath.Length > 0 ? $"{existingFilePath.Length}_{attachment.FileName}" : $"{attachment.FileName}";

            attachment.Path = dirPath;
            attachment.FileName = finalFileName;

            File.WriteAllBytes(Path.Combine(dirPath, finalFileName), attachment.FileStream);

        }
        public string GetSpecificPathForFile(string fileName, int sourceId)
        {
            var dateNow = DateTime.Now;
            var dirPath = Path.Combine(_appSettings.AttachmentPathFile, sourceId.ToString(), dateNow.Year.ToString(), dateNow.Month.ToString("00"));
            return dirPath;
        }

        public byte[] SetMailAttachemntsAsync(SourceModel sourceModel, Attachment attachment)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var bytes = ReadAllBytes(attachment.Path + "\\" + attachment.FileName);
            return bytes;
        }

        private static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }
    }
}
