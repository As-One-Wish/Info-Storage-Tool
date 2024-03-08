using Hwj.SecretVault.HttpApi.Host.Configurations;
using Hwj.SecretVault.Infra.Entity.Shared.Settings;
using Winton.Extensions.Configuration.Consul;

namespace Hwj.SecretVault.HttpApi.Host
{
    /// <summary>
    /// ������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ��ʽ1��Consul����

            builder.Configuration.AddJsonFile("consulAppsettings.json");

            ConsulConfigurationAppsetting? consulConfigurationAppsetting = builder.Configuration.GetSection("ConsulConfigurationAppsetting").Get<ConsulConfigurationAppsetting>();
            if (consulConfigurationAppsetting != null)
            {
                foreach (var itemSetting in consulConfigurationAppsetting.DictionarySettingFiles)
                {
                    builder.Configuration.AddConsul(itemSetting.Value, op =>
                    {
                        op.ConsulConfigurationOptions = cc => cc.Address = new Uri(consulConfigurationAppsetting.Server);
                        op.ReloadOnChange = true;
                    });
                }
            }

            #endregion ��ʽ1��Consul����

            #region ��ʽ2������json�ļ�����

            //builder.Configuration.AddJsonFile("localAppsettings.json");

            #endregion ��ʽ2������json�ļ�����

            bool enableSwagger = bool.Parse(builder.Configuration["EnableSwagger"] ?? "false");

            #region Add services to the container.

            // ���� Consul-������
            builder.Services.AddConsulConfiguration(builder.Configuration);
            // ��ӿ���������
            builder.Services.AddControllers();
            // �����Զ�ע��
            builder.Services.AddAutoInjectConfiguration();
            // ���� FreeSql
            builder.Services.AddFreeSqlConfiguration(builder.Configuration);
            // ���� AutoMapper
            builder.Services.AddAutoMapperConfiguration();
            // ����Jwt�����֤
            builder.Services.AddJwtConfiguration(builder.Configuration);
            // ������������
            builder.Services.AddOtherConfiguration(builder.Configuration);
            // ����Swagger
            builder.Services.AddSwaggerConfiguration(enableSwagger);
            // ������֤��
            builder.Services.AddValidatorConfiguration();

            #endregion Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseSwaggerSetup(enableSwagger);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication(); // ��Ȩ-��ȡ���������û���Ϣ
            app.UseAuthorization(); // ��Ȩ-�����û���Ϣ����û�Ȩ��

            app.MapControllers();

            app.Run();
        }
    }
}