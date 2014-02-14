using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using FizzWare.NBuilder;
using Machine.Fakes;
using Machine.Specifications;
using PagedList;
using SeekMail.Controllers;
using SeekMail.Domain;
using SeekMail.Models;
using SeekMail.Services;

namespace SeekMail.Specs.Controllers
{
    [Subject(typeof(MessagesController))]
    public class When_retrieving_messages_for_page_zero : WithSubject<MessagesController>
    {
        It should_return_bad_request = () =>
            Subject.Get(0).StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

        Establish context = () =>
            Subject.Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/messages");
    }

    [Subject(typeof(MessagesController))]
    public class When_retrieving_messages_for_negative_page_number : WithSubject<MessagesController>
    {
        It should_return_bad_request = () =>
            Subject.Get(-3).StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

        Establish context = () =>
            Subject.Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/messages");
    }

    [Subject(typeof (MessagesController))]
    public class When_retrieving_messages_for_first_page : WithSubject<MessagesController>
    {
        It should_return_messages_from_repository = () =>
            MessagesInResponse.SequenceEqual(The<IPagedList<Message>>().ToList()).ShouldBeTrue();

        It should_provide_next_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldContain(x => x.EndsWith("rel=next"));

        It should_not_provide_previous_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldNotContain(x => x.EndsWith("rel=previous"));

        Establish context = () =>
        {
            var messages = Builder<Message>.CreateListOfSize(100).Build();
            Configure<IPagedList<Message>>(new PagedList<Message>(messages, 1, MessagesController.PageSize));
            The<IMessagesRepository>().WhenToldTo(x => x.GetPage(1, MessagesController.PageSize))
                .Return(The<IPagedList<Message>>());

            // Guff for mocking UrlHelper
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/messages");
            var route = config.Routes.MapHttpRoute("MessagesApi", "api/messages/{page}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "messages" } });
            Subject.ControllerContext = new HttpControllerContext(config, routeData, request);
            Subject.Request = request;
            Subject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            Subject.Url = new UrlHelper(request);
        };

        Because of = () =>
        {
            Response = Subject.Get();
            MessagesInResponse = (IEnumerable<Message>)((ObjectContent)Response.Content).Value;
        };

        static IEnumerable<Message> MessagesInResponse;
        static HttpResponseMessage Response;
    }

    [Subject(typeof(MessagesController))]
    public class When_retrieving_messages_for_middle_page : WithSubject<MessagesController>
    {
        It should_provide_next_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldContain(x => x.EndsWith("rel=next"));

        It should_provide_previous_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldContain(x => x.EndsWith("rel=previous"));

        Establish context = () =>
        {
            var messages = Builder<Message>.CreateListOfSize(100).Build();
            Configure<IPagedList<Message>>(new PagedList<Message>(messages, PageNumber, MessagesController.PageSize));
            The<IMessagesRepository>().WhenToldTo(x => x.GetPage(PageNumber, MessagesController.PageSize))
                .Return(The<IPagedList<Message>>());

            // Guff for mocking UrlHelper
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/messages");
            var route = config.Routes.MapHttpRoute("MessagesApi", "api/messages/{page}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "messages" } });
            Subject.ControllerContext = new HttpControllerContext(config, routeData, request);
            Subject.Request = request;
            Subject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            Subject.Url = new UrlHelper(request);
        };

        Because of = () =>
            Response = Subject.Get(PageNumber);

        static HttpResponseMessage Response;
        const int PageNumber = 2;
    }

    [Subject(typeof(MessagesController))]
    public class When_retrieving_messages_for_last_page : WithSubject<MessagesController>
    {
        It should_not_provide_next_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldNotContain(x => x.EndsWith("rel=next"));

        It should_provide_previous_page_link = () =>
            Response.Headers.Single(x => x.Key == "Link").Value.ShouldContain(x => x.EndsWith("rel=previous"));

        Establish context = () =>
        {
            var messages = Builder<Message>.CreateListOfSize(100).Build();
            Configure<IPagedList<Message>>(new PagedList<Message>(messages, PageNumber, MessagesController.PageSize));
            The<IMessagesRepository>().WhenToldTo(x => x.GetPage(PageNumber, MessagesController.PageSize))
                .Return(The<IPagedList<Message>>());

            // Guff for mocking UrlHelper
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/messages");
            var route = config.Routes.MapHttpRoute("MessagesApi", "api/messages/{page}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "messages" } });
            Subject.ControllerContext = new HttpControllerContext(config, routeData, request);
            Subject.Request = request;
            Subject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            Subject.Url = new UrlHelper(request);
        };

        Because of = () =>
            Response = Subject.Get(PageNumber);

        static HttpResponseMessage Response;
        const int PageNumber = 10;
    }

    [Subject(typeof (MessagesController))]
    public class When_posting_a_new_message : WithSubject<MessagesController>
    {
        It should_send_using_mail_service = () =>
            The<IMailService>().WasToldTo(x => x.Send(The<MessageTemplate>(), EmailAddress));

        It should_add_message_to_repository = () =>
            The<IMessagesRepository>().WasToldTo(x => x.Add(The<Message>()));

        It should_save_changes = () =>
            The<IMessagesRepository>().WasToldTo(x => x.SaveChanges());

        It should_return_ok = () =>
            Response.ShouldBeOfType<OkResult>();

        Establish context = () =>
        {
            The<IMessageTemplateRepository>().WhenToldTo(x => x.Get(TemplateId))
                .Return(The<MessageTemplate>());
            The<IMailService>().WhenToldTo(x => x.Send(Param<IMessageTemplate>.IsAnything, Param<string>.IsAnything))
                .Return(The<Message>());
        };

        Because of = () =>
            Response = Subject.Post(TemplateId, new PostMessageModel { EmailAddress = EmailAddress });

        static IHttpActionResult Response;
        const int TemplateId = 123;
        const string EmailAddress = "joe@bloggs.com";
    }

    [Subject(typeof (MessagesController))]
    public class When_posting_a_new_message_and_model_state_is_invalid : WithSubject<MessagesController>
    {
        It should_return_bad_request_with_model_state = () =>
            Response.ShouldBeOfType<InvalidModelStateResult>();

        Establish context = () =>
            Subject.ModelState.AddModelError("EmailAddress", "The email address entered is invalid.");

        Because of = () =>
            Response = Subject.Post(123, new PostMessageModel());

        static IHttpActionResult Response;
    }

    [Subject(typeof (MessagesController))]
    public class When_posting_a_new_message_and_template_id_does_not_exist : WithSubject<MessagesController>
    {
        It should_return_not_found = () =>
            Response.ShouldBeOfType<NotFoundResult>();

        Establish context = () =>
            The<IMessageTemplateRepository>().WhenToldTo(x => x.Get(123))
                .Return((MessageTemplate)null);

        Because of = () =>
            Response = Subject.Post(123, new PostMessageModel());

        static IHttpActionResult Response;
    }
}