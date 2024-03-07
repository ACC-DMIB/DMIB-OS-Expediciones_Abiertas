Imports System.ComponentModel

Public Class Token

    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property token_type As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property expires_in As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property expires_on As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property not_before As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property resource As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property access_token As String
    <Browsable(True), NotifyParentProperty(True), TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> Public Property expiration_date As Date


End Class
