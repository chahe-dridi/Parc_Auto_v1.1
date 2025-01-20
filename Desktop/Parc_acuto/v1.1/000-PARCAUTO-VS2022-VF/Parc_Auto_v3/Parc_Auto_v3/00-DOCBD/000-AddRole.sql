

-- footer Version  Shared / _Layout.cshtml :  2024 - parc_auto_v1 
--super Admin  id  : ccf8789f-2f15-4c30-92e2-0353ad87693f

  SELECT id, * FROm  [parc_auto].[dbo].[AspNetRoles]  where name = 'superadmin'

    SELECT id,* FROm  [parc_auto].[dbo].[AspNetUsers]  where username in ( 'SAMI_ESSID@BT.COM.TN', '1362@bt.com.tn')



	

	--add 1362 elyes 
	INSERT INTO [parc_auto].[dbo].[AspNetUserRoles]  VALUES ( 'f10e2901-e7a5-4459-93cf-57ec3d265521', 'ccf8789f-2f15-4c30-92e2-0353ad87693f')

	-- Initall :   delete FROm  [parc_auto].[dbo].[AspNetUsers]  where username not  in ( 'SAMI_ESSID@BT.COM.TN', '1362@bt.com.tn')
 



 --***  Req Demandes Null 

 SELECT [d].[Id], [d].[Accompagnateur], [d].[AffectationDepartement], [d].[DateArrivee], [d].[DateDepart], [d].[Description], [d].[Destination], [d].[Etat], [d].[IdEmploye], [d].[Kilometrage], [d].[Mission], [d].[Nom], [d].[Prenom], [d].[TypeVoiture], [d].[UserId], [d].[VoitureId], [v].[Id], [v].[Carburant], [v].[Disponibilite], [v].[Km], [v].[MarqueId], [v].[Matricule], [v].[ModeleId], [v].[NumeroSerieCarteGrise], [v].[TypeVoiture]
FROM [Demandes] AS [d]
LEFT JOIN [Voitures] AS [v] ON [d].[VoitureId] = [v].[Id]


--supprimer les null delete [Demandes] where id = 46

SELECT *   FROM [parc_auto].[dbo].[Demandes]  where Accompagnateur is null 
update [parc_auto].[dbo].[Demandes] set Accompagnateur =' ' where Accompagnateur is null  


/*


---*******  update  Trigger 

CREATE   TRIGGER [dbo].[TR_UPDATEDEMANDE] ON [dbo].[demandes]
AFTER UPDATE
AS
 BEGIN TRY
	update [parc_auto].[dbo].[Demandes] set Accompagnateur ='***'  FROm Inserted where Inserted.Accompagnateur is null 
   END TRY
    BEGIN CATCH
    PRINT 'Error on line Insert  demandes : ' + CAST(ERROR_LINE() AS VARCHAR(10))
    PRINT ERROR_MESSAGE()
END CATCH

      
GO

ALTER TABLE [dbo].[demandes] ENABLE TRIGGER [TR_UPDATEDEMANDE]
GO


*****  Insert   Trigger 

USE [parc_auto]
GO

/****** Object:  Trigger [dbo].[TR_INSERDEMANDE]    Script Date: 14/10/2024 14:45:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE   TRIGGER [dbo].[TR_INSERDEMANDE] ON [dbo].[Demandes]
AFTER INSERT
AS
 BEGIN TRY
	update [parc_auto].[dbo].[Demandes] set Accompagnateur ='***'  FROm Inserted where Inserted.Accompagnateur is null 
   END TRY
    BEGIN CATCH
    PRINT 'Error on line Insert  demandes : ' + CAST(ERROR_LINE() AS VARCHAR(10))
    PRINT ERROR_MESSAGE()
END CATCH

      
GO

ALTER TABLE [dbo].[Demandes] ENABLE TRIGGER [TR_INSERDEMANDE]
GO

 


 




*/