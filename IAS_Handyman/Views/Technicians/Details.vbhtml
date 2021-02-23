@ModelType IAS_Handyman.Models.Technician
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
