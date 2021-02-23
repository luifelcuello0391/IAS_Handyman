@ModelType IAS_Handyman.Models.Technician
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Technician</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Identification)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Identification)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.FullName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.FullName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.CreationDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CreationDateTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ModificationDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ModificationDateTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.RegisterStatus)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.RegisterStatus)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
