using AutoMapper;
using NotificationSystem.Models.Email;
using NotificationSystem.Models.Email.Request;
using NotificationSystem.Models.Source;

namespace NotificationSystem.BusinessLogic.Utils
{
    internal static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(conf =>
            {
                conf.CreateMap<ScheduledEmail, EmailQueue>()
                    .ForMember(email => email.Recipients, opt => opt.MapFrom(email => string.Join(";", email.Recipients)))
                    .ForMember(email => email.CCRecipients, opt => opt.MapFrom(email => string.Join(";", email.CCRecipients)))
                    .ForMember(email => email.BCCRecipients, opt => opt.MapFrom(email => string.Join(";", email.BCCRecipients)))
                    .ForMember(email => email.HasAttachment, opt => opt.MapFrom(email => email.Attachments.Any()));
                conf.CreateMap<SourceDto, SourceModel>()
                    .ForMember(source => source.RecipientsWhiteList, opt => opt.MapFrom(sourceDto=> !string.IsNullOrEmpty(sourceDto.RecipientsWhiteList)? sourceDto.RecipientsWhiteList.Split(";", StringSplitOptions.None): null));

            });
            var mapper = new Mapper(config);
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
