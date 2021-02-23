Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports IAS_Handyman.Models
Imports IAS_Handyman.Repository

Namespace Controllers
    Public Class TechniciansController
        Inherits System.Web.Mvc.Controller

        Private db As New DatabaseContext

        ' GET: Technicians
        Function Index() As ActionResult
            Return View(db.Technicians.ToList())
        End Function

        ' GET: Technicians/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim technician As Technician = db.Technicians.Find(id)
            If IsNothing(technician) Then
                Return HttpNotFound()
            End If
            Return View(technician)
        End Function

        ' GET: Technicians/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Technicians/Create
        'To protect from overposting attacks, enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,Identification,FullName,CreationDateTime,ModificationDateTime,RegisterStatus")> ByVal technician As Technician) As ActionResult
            If ModelState.IsValid Then
                db.Technicians.Add(technician)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(technician)
        End Function

        ' GET: Technicians/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim technician As Technician = db.Technicians.Find(id)
            If IsNothing(technician) Then
                Return HttpNotFound()
            End If
            Return View(technician)
        End Function

        ' POST: Technicians/Edit/5
        'To protect from overposting attacks, enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,Identification,FullName,CreationDateTime,ModificationDateTime,RegisterStatus")> ByVal technician As Technician) As ActionResult
            If ModelState.IsValid Then
                db.Entry(technician).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(technician)
        End Function

        ' GET: Technicians/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim technician As Technician = db.Technicians.Find(id)
            If IsNothing(technician) Then
                Return HttpNotFound()
            End If
            Return View(technician)
        End Function

        ' POST: Technicians/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim technician As Technician = db.Technicians.Find(id)
            db.Technicians.Remove(technician)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
