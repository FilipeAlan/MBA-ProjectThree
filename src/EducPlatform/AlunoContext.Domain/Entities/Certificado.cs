﻿using AlunoContext.Domain.Aggregates;
using BuildingBlocks.Common;

namespace AlunoContext.Domain.Entities;

public class Certificado : EntityBase
{
    public Guid MatriculaId { get; private set; }
    public string CodigoVerificacao { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public Guid AlunoId { get; set; } 
    public Aluno Aluno { get; set; }

    protected Certificado() : base("SYSTEM") { }

    public Certificado(Guid matriculaId, string codigoVerificacao, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        MatriculaId = matriculaId;
        CodigoVerificacao = codigoVerificacao;
        DataEmissao = DateTime.UtcNow;
    }
}
