﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="GuestBookDemo" generation="1" functional="0" release="0" Id="1c240ca0-9b61-4d4b-a9ab-644c7d967023" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="GuestBookDemoGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="GuestBook_WebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/GuestBookDemo/GuestBookDemoGroup/LB:GuestBook_WebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="GuestBook_WebRole:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WebRole:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="GuestBook_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="GuestBook_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="GuestBook_WorkerRole:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WorkerRole:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="GuestBook_WorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="GuestBook_WorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/GuestBookDemo/GuestBookDemoGroup/MapGuestBook_WorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:GuestBook_WebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapGuestBook_WebRole:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRole/DataConnectionString" />
          </setting>
        </map>
        <map name="MapGuestBook_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapGuestBook_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRoleInstances" />
          </setting>
        </map>
        <map name="MapGuestBook_WorkerRole:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRole/DataConnectionString" />
          </setting>
        </map>
        <map name="MapGuestBook_WorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapGuestBook_WorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="GuestBook_WebRole" generation="1" functional="0" release="0" software="D:\Projects\Windows Azure\GuestBookDemo\GuestBookDemo\csx\Release\roles\GuestBook_WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;GuestBook_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;GuestBook_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;GuestBook_WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="GuestBook_WorkerRole" generation="1" functional="0" release="0" software="D:\Projects\Windows Azure\GuestBookDemo\GuestBookDemo\csx\Release\roles\GuestBook_WorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;GuestBook_WorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;GuestBook_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;GuestBook_WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="GuestBook_WebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="GuestBook_WorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="GuestBook_WebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="GuestBook_WorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="GuestBook_WebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="GuestBook_WorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a47c5547-167c-490c-9baa-79c80f625e68" ref="Microsoft.RedDog.Contract\ServiceContract\GuestBookDemoContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="c655857b-bbb0-4feb-a546-3d76fbd899ea" ref="Microsoft.RedDog.Contract\Interface\GuestBook_WebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/GuestBookDemo/GuestBookDemoGroup/GuestBook_WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>