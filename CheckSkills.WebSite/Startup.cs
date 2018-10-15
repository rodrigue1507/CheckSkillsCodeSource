using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckSkills.DAL;
using CheckSkills.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;

namespace CheckSkills.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object UrlParameter { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAnswerDao, AnswerDao>(); 
            services.AddTransient<IQuestionDao, QuestionDao>(); 
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "QuestionList",
                    template: "liste-de-questions",
                    defaults: new { Controller = "Question", Action = "List" });

                routes.MapRoute(
                    name: "CreateQuestion",
                    template: "creation-d-une-question",
                    defaults: new { Controller = "Question", Action = "Create" });

                routes.MapRoute(
                    name: "EditQuestion",
                    template: "modifier-une-question",
                    defaults: new { Controller = "Question", Action = "Details" });

                routes.MapRoute(
                   name: "ConfirmDeleteQuestion",
                   template: "Confirmation-suppression",
                   defaults: new { Controller = "Question", Action = "ConfirmDeleteOrNo" });

                routes.MapRoute(
                  name: "DeleteQuestion",
                  template: "supprimer-une-question",
                  defaults: new { Controller = "Question", Action = "ConfirmDeleteOrNo" });

                routes.MapRoute(
                                name: "savequestion",
                                template: "sauvegarde-question",
                                defaults: new { Controller = "Question", Action = "SaveNewCreate" });

                routes.MapRoute(
                                 name: "ListSurvey",
                                 template: "liste-formulaire",
                                 defaults: new { Controller = "Survey", Action = "SurveyList" });

                routes.MapRoute(
                                name: "SaveSurvey",
                                template: "sauvergarde-formulaire",
                                defaults: new { Controller = "Survey", Action = "SaveSurvey" });



                routes.MapRoute(
                                name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");

            });

            RotativaConfiguration.Setup(env);
        }
    }
}
