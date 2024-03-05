using Info.Storage.HttpApi.Host.Configurations;

namespace Info.Storage.HttpApi.Host
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
            builder.Configuration.AddJsonFile("localAppsettings.json");

            bool enableSwagger = bool.Parse(builder.Configuration["EnableSwagger"] ?? "false");

            #region Add services to the container.

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